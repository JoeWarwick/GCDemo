using CDWRepository;
using CDWSVCAPI.Caching;
using CDWSVCAPI.Helpers;
using FeedParsing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CDWSVCAPI.Services
{
    public class FeedService : IFeedService
    {
        public CDWSVCModel Model { get; }

        public ILogger Logger { get; }

        public AutoFeedRefreshCache Cache { get; }

        public UserManager<CDWSVCUser> UserManager { get; }

        public FeedService(CDWSVCModel model, AutoFeedRefreshCache cache, UserManager<CDWSVCUser> userManager, ILogger logger)
        {
            this.Model = model;
            this.Cache = cache;
            this.Logger = logger;
            this.UserManager = userManager;
        }


        public async Task<IList<Item>> GetEntries(Guid usr, string hash, int id)
        {
            var user = Model.CDWSVCUsers.FirstOrDefault(u => u.Id == usr.ToString()) as CDWSVCUser;
            var fp = new FeedParser();
            var res = new List<Item>();
            var feed = Model.Subscribables.FirstOrDefault(s => s.Id == id && s.Owner.Id == usr.ToString());
            if (feed == null) return res;
            var url = "~/feed/" + usr + "/" + hash + "/" + feed.Id;
            try
            {
                switch (feed.FeedType.TypeName)
                {
                    case "ATOM": return await fp.Parse(new Uri(url), FeedType.Atom);
                    case "RSS": return await fp.Parse(new Uri(url), FeedType.RSS);
                    case "RDF": return await fp.Parse(new Uri(url), FeedType.RDF);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, string.Format("Feed Parse failed for url {0}", url));
            }
            if (fp.Errors.Count() > 0)
            {
                Logger.LogWarning(string.Format("Entries: {0} failed.\n {1}", fp.Errors.Count(), string.Join("\n", fp.Errors)));
            }
            return res;
        }

        public async Task<string> GetFeed(Guid usr, string hash, int id, string accept, string fmt = "")
        {
            await Model.Subscribables.OfType<FeedSet>().Include("Feeds").LoadAsync();

            var user = await Model.CDWSVCUsers.FirstOrDefaultAsync(u =>
                u.Id == usr.ToString() && u.HashStr == hash
            );
            if (user == null)
            {
                return "No such user found!";
            }
            
            
            Subscribable feed = await Model.Subscribables
                .Include("FeedSource.Group.Params")
                .Include("FeedSource.Group.FeedTransforms.InputFeedType")
                .FirstOrDefaultAsync(f => f.Id == id && f.Owner.Id == usr.ToString());
            if (feed == null) return null;
            if (feed.FeedSource.LastChange > feed.Added)
            {
                var newfeed = DBInitialiser.CreateFromSource(feed.FeedSource, user);
                feed.Url = newfeed.Url;
                feed.Added = DateTime.Now;
                await Model.SaveChangesAsync();
                Cache.Remove(Tuple.Create("Raw", feed.Id));
            }

            var resp = Cache.Get(Tuple.Create("Raw", feed.Id)).OuterXml;

            if (!string.IsNullOrEmpty(fmt))
            {
                if (fmt.ToUpperInvariant() == "RSS" && feed.FeedType.TypeName != "RSS")
                {
                    var fp = new FeedParser();
                    var temp = fp.Parse(resp, FeedType.Atom) as List<Item>;
                    Stream mem = new MemoryStream();
                    fp.Write(ref mem, temp, FeedType.RSS, feed.Name, feed.Description, feed.WebUrl);
                    resp = Encoding.UTF8.GetString((mem as MemoryStream).ToArray());
                }
            }
            //detect json or xml requested
            
            if (accept == "application/json")
            {
                var xdoc = new XmlDocument();
                xdoc.LoadXml(resp);
                return JsonConvert.SerializeXmlNode(xdoc);
            }
            return resp;
        }

        public async Task<bool> IsPremium(Guid usr, string hash)
        {
            var user = await Model.CDWSVCUsers.FirstOrDefaultAsync(u =>
               u.Id == usr.ToString() && u.HashStr == hash);

           return await UserManager.IsInRoleAsync(user, "PremiumUser");
        }

        public async Task<bool> IsValidUser(Guid usr, string hash)
        {
            var user = await Model.CDWSVCUsers.FirstOrDefaultAsync(u =>
               u.Id == usr.ToString() && u.HashStr == hash);
            return BLHelper.isValidUser(user, hash);
        }
    }
}
