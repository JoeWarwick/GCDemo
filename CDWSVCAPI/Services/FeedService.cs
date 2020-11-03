using CDWSVCAPI.Caching;
using FeedParsing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CDWRepository.CDWSVCModel;

namespace CDWSVCAPI.Services
{
    public class FeedService : IFeedService
    {
        public CDWRepository.CDWSVCModel Model { get; }
        public ILogger Logger { get; }
        public AutoFeedRefreshCache Cache { get; }
        
        public FeedService(CDWRepository.CDWSVCModel model, AutoFeedRefreshCache cache, ILogger logger)
        {
            this.Model = model;
            this.Cache = cache;
            this.Logger = logger;

        }


        public IList<Item> GetEntries(Guid usr, string hash, int id)
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
                    case "ATOM": return fp.Parse(new Uri(url), FeedType.Atom);
                    case "RSS": return fp.Parse(new Uri(url), FeedType.RSS);
                    case "RDF": return fp.Parse(new Uri(url), FeedType.RDF);
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

        public async Task<string> GetFeed(Guid usr, string hash, int id, string fmt = "")
        {
            await Model.Subscribables.OfType<FeedSet>().Include("Feeds").LoadAsync();

            var user = await Model.CDWSVCUsers.FirstOrDefaultAsync(u =>
                u.Id == usr.ToString() && u.HashStr == hash
            );
            if (user == null)
            {
                return "No such user found!";
            }
            var isPremium = await Model.Roles.AnyAsync(r => r.Name == "PremiumUser" && r.Users.Any(u => u.UserId == user.Id));
            if (!BLL.isValidUser(user)) return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            Subscribable feed = await Model.Subscribables
                .Include("FeedSource.Group.Params")
                .Include("FeedSource.Group.FeedTransforms.InputFeedType")
                .FirstOrDefaultAsync(f => f.Id == id && f.Owner.Id == usr.ToString());
            if (feed == null) return new HttpResponseMessage(HttpStatusCode.NotFound);
            if (feed.FeedSource.LastChange > feed.Added)
            {
                var newfeed = DBInitialiser.CreateFromSource(feed.FeedSource, user);
                feed.Url = newfeed.Url;
                feed.Added = DateTime.Now;
                await Model.SaveChangesAsync();
                _cache.Remove(Tuple.Create("Raw", feed.Id));
            }

            var resp = _cache.Get(Tuple.Create("Raw", feed.Id)).OuterXml;

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
            IEnumerable<string> headerValues;
            //detect json or xml requested
            var accept = string.Empty;
            var keyFound = Request.Headers.TryGetValues("accept", out headerValues);
            if (keyFound)
            {
                accept = headerValues.FirstOrDefault();
            }
            if (accept == "application/json")
            {
                var xdoc = new XmlDocument();
                xdoc.LoadXml(resp);
                return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeXmlNode(xdoc), Encoding.UTF8, "application/json") };
            }
            return new HttpResponseMessage() { Content = new StringContent(resp, Encoding.UTF8, "application/xml") };
        }
    }
}
