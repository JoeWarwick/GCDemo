using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CDWRepository;
using System.Linq;


namespace CDWRepository
{
    public class DBInitialiser
        // : DropCreateDatabaseIfModelChanges<CDWSVCModel>
        // : DropCreateDatabaseAlways<CDWSVCModel>
        // : CreateDatabaseIfNotExists<CDWSVCModel>
    {

        private const string _adminUserName = "admin@CDWSVC.com";
        private const string _baseUserName = "jrwarwick@hotmail.com";
        private const string _baseUserHash = "sdFSSHLFfdllffl";
        private const string _premUserName = "bdeyed3dguy@gmail.com";
        private const string _premUserHash = "GqFbOyAZZkrksdr";

        //protected override void Seed(CDWSVCModel context)
        //{
        //    SqlConnection.ClearAllPools();
        //    InitializeIdentityForEF(context);
        //    InitializeFeedSystem(context);
        //    base.Seed(context);
        //}

        //protected async Task InitializeIdentityForEF(CDWSVCModel db)
        //{
        //    var userManager = new UserManager<CDWSVCUser>();
        //    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole<string>>(db));
        //    const string name = _adminUserName;
        //    const string password = "Sn0rkl3r!";
        //    const string roleName = "Admin";
        //    const string baseRoleName = "WallpaperUser";
        //    const string premRoleName = "PremiumUser";

        //    //Create Role Admin if it does not exist
        //    IdentityRole role;
        //    if (!roleManager.RoleExists(roleName))
        //    {
        //        role = new IdentityRole(roleName);
        //        var roleresult = roleManager.Create(role);
        //        role = new IdentityRole(baseRoleName);
        //        roleresult = roleManager.Create(role);
        //        role = new IdentityRole(premRoleName);
        //        roleresult = roleManager.Create(role);
        //    }

        //    var user = userManager.FindByName(name);
        //    if (user == null)
        //    {
        //        user = new CDWSVCUser { UserName = name, Email = name };
        //        var result = userManager.Create(user, password);
        //        result = userManager.SetLockoutEnabled(user.Id, false);
        //        // Add user admin to Role Admin if not already added
        //        var rolesForUser = userManager.GetRoles(user.Id);
        //        if (!rolesForUser.Contains(roleName))
        //        {
        //            result = userManager.AddToRole(user.Id, roleName);
        //        }
        //        user = new CDWSVCUser { UserName = _baseUserName, Email = _baseUserName, HashStr = _baseUserHash };
        //        result = userManager.Create(user, password);
        //        userManager.SetLockoutEnabled(user.Id, false);
        //        rolesForUser = userManager.GetRoles(user.Id);
        //        if (!rolesForUser.Contains(baseRoleName))
        //        {
        //            result = userManager.AddToRole(user.Id, baseRoleName);
        //        }
        //        user = new CDWSVCUser { UserName = _premUserName, Email = _premUserName, HashStr = _premUserHash };
        //        result = userManager.Create(user, password);
        //        userManager.SetLockoutEnabled(user.Id, false);
        //        rolesForUser = userManager.GetRoles(user.Id);
        //        if (!rolesForUser.Contains(baseRoleName))
        //        {
        //            result = userManager.AddToRole(user.Id, baseRoleName);
        //        }
        //        if (!rolesForUser.Contains(premRoleName))
        //        {
        //            result = userManager.AddToRole(user.Id, premRoleName);
        //        }
        //    }
        //}

        //protected void InitializeFeedSystem(CDWSVCModel context)
        //{
        //    var userManager = new ApplicationUserManager(new UserStore<CDWSVCUser>(context));
        //    var adminUser = userManager.FindByEmail(_adminUserName) as CDWSVCUser;
        //    var baseUser = userManager.FindByEmail(_baseUserName) as CDWSVCUser;
        //    var premUser = userManager.FindByEmail(_premUserName) as CDWSVCUser;

        //    var ft1 = context.FeedTypes.Add(new DbFeedType { TypeName = "ATOM", Version = "2.0" });
        //    var ft2 = context.FeedTypes.Add(new DbFeedType { TypeName = "RSS", Version = "2.0" });
        //    var reddit = new FeedTransform() { Name = "Reddit fixer", Url = "~/XSLT/Reddit.xslt", Shared = true, InputFeedType = ft1, Owner = premUser };
        //    var bing = new FeedTransform() { Name = "Bing fixer", Url = "~/XSLT/Bing.xslt", Shared = true, InputFeedType = ft2, Owner = premUser };
        //    var utf8 = new FeedTransform() { Name = "Flickr fixer", Url = "~/XSLT/CUTF8.xslt", Shared = true, InputFeedType = ft1, Owner = premUser };
        //    var flickr = new FeedTransform() { Name = "Flickr fixer", Url = "~/XSLT/Flickr.xslt", Shared = true, InputFeedType = ft1, Owner = premUser };
        //    var tumblr = new FeedTransform() { Name = "RSS Inner Content", Url = "~/XSLT/InnerSrcRSS.xslt", Shared = true, InputFeedType = ft2, Owner = premUser };
        //    var phone = new FeedTransform() { Name = "Atom Content Link Only", Url = "~/XSLT/AnchorFromContent.xslt", Shared = true, InputFeedType = ft1, Owner = premUser };
        //    var ftg1 = new FeedTag { Tag = "HD" };
        //    var grp = context.FeedSourceGroups.Add(new FeedSourceGroup
        //    {
        //        Name = "Reddit (safe for work screens)",
        //        Site = "Reddit",
        //        Logo = "http://www.lsdi.it/assets/reddit-alien.png",
        //        BaseUri = "https://www.reddit.com/r/",
        //        Description = "Reddit is like a popularity contest for best posts",
        //        ShortName = "Reddit SFW",
        //        Shared = true,
        //        Adult = false,
        //        Rating = 5,
        //        Owner = adminUser,
        //        FeedTransforms = new List<FeedTransform> { { reddit } },
        //        FormatUrlString = "https://www.reddit.com/r/{subreddit}.rss?sort={sort}&top={top}&t={time}",
        //        Params = new List<Param> { { new Param { Key = "subreddit", Desc = "Reddit Subreddit(s) to include", Type = "subreddits", Required=true } }, { new Param { Key = "top", Desc = "Entries to Grab", Type = "positiveInteger", Value = "20" } },
        //            { new Param { Key="sort", Desc="hot,new,rising,controversial,top,gilded", Type="choice", Value="hot" } }, { new Param { Key="time", Desc="hour,day,week,month,year,all", Type="choice", Value="day" } } }
        //    });

        //    var fs1 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Reddit Earthporn",
        //        ShortName = "earthporn",
        //        Adult = false,
        //        ProducesArtifact = true,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft1,
        //        Description = "Pictures of natural landscapes.",
        //        Params = new List<Param> { { new Param { Key = "subreddit", Value = "earthporn" } } },
        //        Rating = 5,
        //        Owner = premUser,
        //        WebUrl = "https://www.reddit.com/r/earthporn",
        //        FeedBaseUrl = "https://www.reddit.com/r/earthporn.rss",
        //        Tags = new List<FeedTag> { { new FeedTag { Tag = "Nature" } }, new FeedTag { Tag = "Landscape" } },
        //        ExampleImages = new List<FeedImage> { { new FeedImage { Url = "http://imgur.com/AmWThvw", Score=12639, Attribution="OurEarthInFocus", Published = new DateTime(2015, 8,23),
        //            RelLink = "http://i.imgur.com/AmWThvw.jpg", Caption ="I needed a spot to sleep while on a recent road trip and found an epic vantage point above Snoqualmie Pass, WA. Multiple wildfires, and Seattle's light pollution made for a very intense scene!" } } }
        //    });
        //    var fs2 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Reddit Space",
        //        ShortName = "spaceporn",
        //        Adult = false,
        //        ProducesArtifact = true,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft1,
        //        Description = "A subreddit devoted to high-quality images of space.",
        //        Params = new List<Param> { { new Param { Key = "subreddit", Value = "spaceporn" } } },
        //        Rating = 5,
        //        Owner = premUser,
        //        WebUrl = "https://www.reddit.com/r/spaceporn",
        //        FeedBaseUrl = "https://www.reddit.com/r/spaceporn.rss",
        //        Tags = new List<FeedTag> { { new FeedTag { Tag = "Space" } }, new FeedTag { Tag = "NASA" }, new FeedTag { Tag = "SpaceEngine" } },
        //        ExampleImages = new List<FeedImage> { { new FeedImage { Url = "http://www.tsene.com/wp-content/uploads/messenger_orbit.jpg", Score=6526, Attribution="ForScale", Published = new DateTime(2013, 2, 10),
        //            RelLink = "http://www.tsene.com/wp-content/uploads/messenger_orbit.jpg", Caption ="Clearest pic of Mercury you have ever seen..." } } }
        //    });
        //    var fs3 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Reddit Cityporn",
        //        ShortName = "cityporn",
        //        Adult = false,
        //        ProducesArtifact = true,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft1,
        //        Description = "A subreddit devoted to cityscapes.",
        //        Params = new List<Param> { { new Param { Key = "subreddit", Value = "cityporn" } } },
        //        Rating = 5,
        //        Owner = premUser,
        //        WebUrl = "https://www.reddit.com/r/cityporn",
        //        FeedBaseUrl = "https://www.reddit.com/r/cityporn.rss",
        //        Tags = new List<FeedTag> { { new FeedTag { Tag = "City" } }, new FeedTag { Tag = "Architecture" }, new FeedTag { Tag = "Cityscape" } },
        //        ExampleImages = new List<FeedImage> { { new FeedImage { Url = "http://i.imgur.com/g33IKI5.jpg", Score=4722, Attribution="biwook", Published = new DateTime(2016, 2, 7),
        //            RelLink = "http://i.imgur.com/g33IKI5.jpg", Caption ="Highways in Tokyo" } } }
        //    });
        //    var fsESC = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Reddit Earth, Space, City",
        //        ShortName = "Reddit E,S,C",
        //        ProducesArtifact = true,
        //        Adult = false,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft1,
        //        Rating = 5,
        //        LoadChildren = false,
        //        Description = "A mix of Earth, Space and Cities.",
        //        Params = new List<Param> { { new Param { Key = "subreddit", Value = "earthporn+spaceporn+cityporn" } } },
        //        FeedSources = new List<FeedSource> { { fs1 }, { fs2 }, { fs3 } },
        //        Owner = premUser,
        //        WebUrl = "https://www.reddit.com/r/earthporn+spaceporn+cityporn?sort=hot&top=20&t=day",
        //        FeedBaseUrl = "https://www.reddit.com/r/earthporn+spaceporn+cityporn.rss",
        //        Tags = new List<FeedTag>().Concat(fs1.Tags).Concat(fs2.Tags).Concat(fs3.Tags).ToList(),
        //        ExampleImages = new List<FeedImage>().Concat(fs1.ExampleImages).Concat(fs2.ExampleImages).Concat(fs3.ExampleImages).ToList()
        //    });

        //    //Feeds and FeedSets are the artifacts of the feed building system - users create feeds off of feedsources - a feed is an instance of a feedsource with the params injected hierarchically into the urls from the child run of feedsources.
        //    context.Subscribables.Add(CreateFromSource(fsESC, baseUser));
        //    context.Subscribables.Add(CreateFromSource(fsESC, premUser));

        //    var grpPh = context.FeedSourceGroups.Add(new FeedSourceGroup
        //    {
        //        Name = "Reddit Phone Wallpapers",
        //        Site = "Reddit",
        //        Logo = "http://www.lsdi.it/assets/reddit-alien.png",
        //        BaseUri = "https://www.reddit.com/r/",
        //        Description = "Reddit is like a popularity contest for best posts",
        //        ShortName = "Reddit SFW",
        //        Shared = true,
        //        Adult = false,
        //        Rating = 5,
        //        Owner = adminUser,
        //        FeedTransforms = new List<FeedTransform> { { reddit }, { phone } },
        //        FormatUrlString = "https://www.reddit.com/r/{subreddit}.rss?sort={sort}&top={top}&t={time}",
        //        Params = new List<Param> { { new Param { Key = "subreddit", Desc = "Reddit Subreddit(s) to include", Type = "subreddits", Required=true } }, { new Param { Key = "top", Desc = "Entries to Grab", Type = "positiveInteger", Value = "20" } },
        //            { new Param { Key="sort", Desc="hot,new,rising,controversial,top,gilded", Type="choice", Value="hot" } }, { new Param { Key="time", Desc="hour,day,week,month,year,all", Type="choice", Value="week" } } }
        //    });
        //    var fsPhone = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Mobile Wallpaper",
        //        ShortName = "Wallpaper",
        //        Adult = false,
        //        Group = grpPh,
        //        Shared = true,
        //        FeedType = ft1,
        //        Description = "Reddit Mobile Wallpaper",
        //        Owner = premUser,
        //        Tags = new List<FeedTag> { new FeedTag { Tag = "Phone" } },
        //        Params = new List<Param> { { new Param { Key = "subreddit", Value = "MobileWallpaper+iWallpaper+VerticalWallpapers+VerticalWallpaper" } }, { new Param { Key = "top", Value = "100" } } },
        //    });

        //    context.Subscribables.Add(CreateFromSource(fsPhone, premUser));

        //    grp = context.FeedSourceGroups.Add(new FeedSourceGroup
        //    {
        //        Name = "Reddit Search",
        //        Logo = "http://www.lsdi.it/assets/reddit-alien.png",
        //        BaseUri = "https://www.reddit.com/r/",
        //        Description = "Reddit is like a popularity contest for best posts",
        //        ShortName = "Reddit Custom",
        //        Shared = true,
        //        Adult = false,
        //        Rating = 3,
        //        Site = "Reddit",
        //        Owner = adminUser,
        //        FeedTransforms = new List<FeedTransform> { { reddit } },
        //        FormatUrlString = "https://www.reddit.com/r/{subreddit}/search.rss?q={query}&sort={sort}&top={top}&t={time}&restrict_sr=on",
        //        Params = new List<Param> { { new Param { Key = "subreddit", Desc = "Reddit Subreddit(s) to include", Type = "subreddits", Required=true } }, { new Param { Key = "query", Desc = "Query to search Reddit", Type = "string", Required=true, Value="3840 2160" } },
        //            { new Param { Key = "top", Desc = "Entries to Grab", Type = "positiveInteger", Value = "20" } }, { new Param { Key="sort", Desc="hot,new,rising,conterversial,top,gilded", Type="choice", Value="hot" } },
        //            { new Param { Key="time", Desc="hour,day,week,month,year,all", Type="choice", Value="year" } } }
        //    });

        //    fs1 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Reddit 1920x1200",
        //        ShortName = "Reddit HD",
        //        Adult = false,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft1,
        //        Description = "Reddit 1920x1200 Image Search",
        //        Params = new List<Param> { { new Param { Key = "query", Value = "1920 x 1200" } } },
        //        Owner = premUser,
        //        Tags = new List<FeedTag> { new FeedTag { Tag = "1200p" }, ftg1 }
        //    });
        //    fs2 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Reddit 1600p HD",
        //        ShortName = "Reddit 1600p",
        //        Adult = false,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft1,
        //        Description = "Reddit 2560x1600 Image Search",
        //        Params = new List<Param> { { new Param { Key = "query", Value = "2560 x 1600" } } },
        //        Owner = premUser,
        //        Tags = new List<FeedTag> { new FeedTag { Tag = "1600p" }, ftg1 }
        //    });
        //    fs3 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Reddit 1800p HD",
        //        ShortName = "Reddit 1800p",
        //        Adult = false,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft1,
        //        Description = "Reddit 3200x1800 Image Search",
        //        Params = new List<Param> { { new Param { Key = "query", Value = "3200 x 1800" } } },
        //        Owner = premUser,
        //        Tags = new List<FeedTag> { new FeedTag { Tag = "1800p" }, ftg1 }
        //    });
        //    var fs4 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Reddit 4K 16:10",
        //        ShortName = "Reddit 4K",
        //        Adult = false,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft1,
        //        Description = "Reddit 3840x2400 Image Search",
        //        Params = new List<Param> { { new Param { Key = "query", Value = "3840 x 2400" } } },
        //        Owner = premUser,
        //        Tags = new List<FeedTag> { new FeedTag { Tag = "4K" }, ftg1 }
        //    });
        //    var fs5 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Reddit 16:10 HD",
        //        ShortName = "Reddit 16:10",
        //        FeedSources = new List<FeedSource> { fs1, fs2, fs3, fs4 },
        //        Shared = true,
        //        FeedType = ft1,
        //        Owner = baseUser,
        //        Group = grp,
        //        Description = "Search for HD images to fit 16:10 resolution",
        //        Tags = new List<FeedTag> { new FeedTag { Tag = "16:10" } }.Union(fs1.Tags).Union(fs2.Tags).Union(fs3.Tags).Union(fs4.Tags).ToList()
        //    });
        //    //non artifact producing FeedSource ^^ No required subreddit param

        //    fs1 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Reddit 1080P",
        //        ShortName = "Reddit 1080P",
        //        Adult = false,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft1,
        //        Description = "Reddit 1920x1080 Image Search",
        //        Params = new List<Param> { { new Param { Key = "query", Value = "1920 x 1080" } } },
        //        Owner = premUser,
        //        Tags = new List<FeedTag> { new FeedTag { Tag = "1080p" }, ftg1 }
        //    });
        //    fs2 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Reddit 1440p HD",
        //        ShortName = "Reddit 1440p",
        //        Adult = false,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft1,
        //        Description = "Reddit 2560x1440 Image Search",
        //        Params = new List<Param> { { new Param { Key = "query", Value = "2560 x 1440" } } },
        //        Owner = premUser,
        //        Tags = new List<FeedTag> { new FeedTag { Tag = "1440p" }, ftg1 }
        //    });
        //    fs3 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Reddit 4K 16:9",
        //        ShortName = "Reddit 4K",
        //        Adult = false,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft1,
        //        Description = "Reddit 3840x2160 Image Search",
        //        Params = new List<Param> { { new Param { Key = "query", Value = "3840 x 2160" } } },
        //        Owner = premUser,
        //        Tags = new List<FeedTag> { new FeedTag { Tag = "2160p" }, ftg1 }
        //    });
        //    fs4 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Reddit 16:9 HD",
        //        ShortName = "Reddit 16:9",
        //        ProducesArtifact = true,
        //        FeedSources = new List<FeedSource> { fs1, fs2, fs3 },
        //        Shared = true,
        //        FeedType = ft1,
        //        Owner = baseUser,
        //        Group = grp,
        //        Description = "Search for HD images to fit 16:9 resolution",
        //        Tags = new List<FeedTag> { new FeedTag { Tag = "16:9" } }.Union(fs1.Tags).Union(fs2.Tags).Union(fs3.Tags).Union(fs4.Tags).ToList()
        //    });
        //    var fsHD = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Reddit 16:9|10 HD Earth, Space, City",
        //        ShortName = "Reddit E,S,P HD",
        //        Adult = false,
        //        FeedType = ft1,
        //        Shared = false,
        //        Owner = premUser,
        //        WebUrl = "https://www.reddit.com/r/earthporn+spaceporn+cityporn?sort=hot&top=20&t=day",
        //        Description = "Reddit 16:9|10 HD Earth, Space, City",
        //        Group = grp,
        //        FeedSources = new List<FeedSource> { fsESC, fs4, fs5 }
        //    });
        //    context.Subscribables.Add(CreateFromSource(fsHD, baseUser));
        //    context.Subscribables.Add(CreateFromSource(fsHD, premUser));

        //    grp = context.FeedSourceGroups.Add(new FeedSourceGroup
        //    {
        //        Name = "Bing!",
        //        Site = "Bing",
        //        Logo = "http://www.abondance.com/actualites/wp-content/uploads/2013/11/b-de-bing1.png",
        //        BaseUri = "http://www.bing.com/HPImageArchive.aspx?format=rss&idx=0&n=15&mkt=en-AU",
        //        Description = "Bing! Picture of the Day",
        //        Shared = true,
        //        Adult = false,
        //        Rating = 4,
        //        Owner = adminUser,
        //        FeedTransforms = new List<FeedTransform> { { bing } },
        //        FormatUrlString = "http://www.bing.com/HPImageArchive.aspx?format=rss&idx={start}&n={count}&mkt={culture}",
        //        Params = new List<Param> { { new Param { Key = "start", Desc = "Start Index", Type = "positiveInteger" } }, { new Param { Key = "count", Desc = "Entries to Grab", Type = "positiveInteger", Value = "7" } } }
        //    });

        //    fs1 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Bing! Pic of the Day 1-7",
        //        ShortName = "Bing!",
        //        Adult = false,
        //        ProducesArtifact = true,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft2,
        //        Description = "Bing! Picture of the Day",
        //        Params = new List<Param> { { new Param { Key = "start", Value = "0" } } },

        //    });
        //    fs2 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Bing! Pic of the Day 8-14",
        //        ShortName = "Bing!",
        //        Adult = false,
        //        ProducesArtifact = true,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft2,
        //        Description = "Bing! Picture of the Day",
        //        Params = new List<Param> { { new Param { Key = "start", Value = "7" } } },

        //    });
        //    fs3 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Bing! Pic of the Day 15-21",
        //        ShortName = "Bing!",
        //        Adult = false,
        //        ProducesArtifact = true,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft2,
        //        Description = "Bing! Picture of the Day",
        //        Params = new List<Param> { { new Param { Key = "start", Value = "14" } }, { new Param { Key = "culture", Value = "en-US" } } },

        //    });
        //    fs4 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Bing! Pic of the Day 21-28",
        //        ShortName = "Bing!",
        //        Adult = false,
        //        ProducesArtifact = true,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft2,
        //        Description = "Bing! Picture of the Day",
        //        Params = new List<Param> { { new Param { Key = "start", Value = "21" } }, { new Param { Key = "culture", Value = "en-US" } } },

        //    });
        //    var fsBing = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Bing! Pic of the Day",
        //        ShortName = "Bing!",
        //        Adult = false,
        //        ProducesArtifact = true,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft2,
        //        Rating = 3,
        //        WebUrl = "http://bing.com/HPImageArchive.aspx?format=rss",
        //        Owner = premUser,
        //        FeedBaseUrl = "http://bing.com/HPImageArchive.aspx?format=rss",
        //        Tags = new List<FeedTag> { { new FeedTag { Tag = "Bing" } }, new FeedTag { Tag = "Daily" } },
        //        FeedSources = new List<FeedSource> { fs1, fs2 },
        //        Params = new List<Param> { { new Param { Key = "culture", Value = "en-AU" } } },
        //        ExampleImages = new List<FeedImage> { { new FeedImage { Url = "http://www.bing.com/az/hprichbg/rb/LastNightProms_EN-AU6602411502_1920x1080.jpg", Score=-1, Attribution="chrisstockphotography/Alamy", Published = new DateTime(2016, 10, 2),
        //            RelLink = "http://www.bing.com/az/hprichbg/rb/LastNightProms_EN-AU6602411502_1366x768.jpg", Caption ="Acoustic sound panels in the ceiling of the Royal Albert Hall, London" } } }
        //    });

        //    context.Subscribables.Add(CreateFromSource(fsBing, baseUser));
        //    context.Subscribables.Add(CreateFromSource(fsBing, premUser));

        //    grp = context.FeedSourceGroups.Add(new FeedSourceGroup
        //    {
        //        Name = "Flickr Groups",
        //        FeedTransforms = new List<FeedTransform> { { flickr } },
        //        Site = "Flickr",
        //        Logo = "http://farm1.staticflickr.com/1/buddyicons/40961104@N00.jpg?1100187620",
        //        BaseUri = "http://www.flickr.com/groups",
        //        Description = "Wallpapers (1024x768 minimum) Pool",
        //        Shared = true,
        //        Adult = false,
        //        Rating = 4,
        //        Owner = adminUser,
        //        FormatUrlString = "http://api.flickr.com/services/feeds/groups_pool.gne?id={groupid}&lang={culture}",
        //        Params = new List<Param> { { new Param { Key = "groupid", Desc = "Flickr Group ID", Type = "string" } } }
        //    });
        //    fs3 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Flickr Wallpapers 1080p",
        //        ShortName = "Flickr 1080p",
        //        ProducesArtifact = true,
        //        Adult = false,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft1,
        //        Description = "Flickr Wallpapers (1024x768 minimum) Pool",
        //        Params = new List<Param> { { new Param { Key = "groupid", Value = "40961104@N00" } } },
        //        Owner = premUser,
        //        WebUrl = "https://www.flickr.com/groups/wallpapers/pool/",
        //        FeedBaseUrl = "http://api.flickr.com/services/feeds/groups_pool.gne",
        //        Rating = 4,
        //        Tags = new List<FeedTag> { { new FeedTag { Tag = "Flickr" } } },
        //        ExampleImages = new List<FeedImage> { { new FeedImage { Url = "http://farm9.staticflickr.com/8141/29935708575_d02035b986_b.jpg", Score=-1, Attribution="arjayempee", Published = new DateTime(2016, 09, 26),
        //            RelLink = "http://www.flickr.com/photos/62445171@N00/29935708575/in/pool-40961104@N00", Caption ="Cullen House (2)" } } }
        //    });
        //    context.Subscribables.Add(CreateFromSource(fs3, baseUser));
        //    context.Subscribables.Add(CreateFromSource(fs3, premUser));

        //    grp = context.FeedSourceGroups.Add(new FeedSourceGroup
        //    {
        //        Name = "Pinterest",
        //        FeedTransforms = new List<FeedTransform> { { utf8 } },
        //        Site = "Pinterest",
        //        Logo = "https://upload.wikimedia.org/wikipedia/commons/0/08/Pinterest-logo.png",
        //        BaseUri = "http://www.pinterest.com/",
        //        Description = "Pinterest walls are a good place to get good wallpapers",
        //        Shared = true,
        //        Adult = false,
        //        Rating = 3,
        //        Owner = adminUser,
        //        FormatUrlString = "http://pinterest.com/{board}/feed.rss",
        //        Params = new List<Param> { { new Param { Key = "board", Desc = "Pinterest member name or 'popular'", Type = "string", Value = "popular" } } }
        //    });

        //    grp = context.FeedSourceGroups.Add(new FeedSourceGroup
        //    {
        //        Name = "Tumblr",
        //        FeedTransforms = new List<FeedTransform> { { tumblr } },
        //        Site = "Tumblr",
        //        Logo = "https://assets.tumblr.com/images/logo_page/img_logo_grey_2x.png",
        //        BaseUri = "http://www.tumblr.com/",
        //        Description = "Tumblr can be a good place to get good wallpapers",
        //        Shared = true,
        //        Adult = false,
        //        Rating = 3,
        //        Owner = adminUser,
        //        FormatUrlString = "http://{board}.tumblr.com/rss",
        //        Params = new List<Param> { { new Param { Key = "board", Desc = "Tumblr member name", Type = "string", Value = "wallpaper" } } }
        //    });
        //    fs3 = context.FeedSources.Add(new FeedSource
        //    {
        //        Name = "Tumblr: God Save the Gold",
        //        ShortName = "GodSaveTheGold",
        //        ProducesArtifact = true,
        //        Adult = false,
        //        Group = grp,
        //        Shared = true,
        //        FeedType = ft2,
        //        Description = "God Save the Gold",
        //        Owner = premUser,
        //        WebUrl = "http://godsavethegold.tumblr.com/",
        //        Rating = 4,
        //        Tags = new List<FeedTag> { { new FeedTag { Tag = "Tumblr" } } },
        //        Params = new List<Param> { { new Param { Key = "board", Value = "godsavethegold" } } }
        //    });
        //    context.Subscribables.Add(CreateFromSource(fs3, baseUser));
        //    context.Subscribables.Add(CreateFromSource(fs3, premUser));

        //    context.SaveChanges();
        //}

        //A feed subscription is an artifact that is generated from a feed source. It is stamped out like a ticket.
        //This code will perhaps need to operate on the client side too 
        public static Subscribable CreateFromSource(FeedSource source, CDWSVCUser user)
        {
            //var avg = new List<int>();
            //if (source.FeedSources == null) source.FeedSources = new List<FeedSource>();
            //var parms = source.Params == null ? source.FeedSources.Where(f => f.Params != null).SelectMany(f => f.Params).ToList() : source.Params.Union(source.FeedSources.Where(f => f.Params != null).SelectMany(f => f.Params)).ToList();
            //var res = CreateFeedFromSource(source, user, parms, avg, true);
            //var rates = avg.Where(v => v > 0);
            //res.Rating = rates.Count() == 0 ? 2 : Convert.ToInt16(rates.Average());
            //return res;
            return new Feed();
        }

        //public static Subscribable CreateFeedFromSource(FeedSource source, CDWSVCUser user, List<Param> parms, List<int> ratings, bool head = false)
        //{
        //    Subscribable res;
        //    //var pc = new ParamComparer();
        //    //if (source.Rating > 0)
        //    //{
        //    //    ratings.Add(source.Rating);
        //    //}
        //    //if (source.FeedSources != null && source.FeedSources.Count() > 1)
        //    //{
        //    //    res = new FeedSet
        //    //    {
        //    //        Name = source.Name,
        //    //        IsArtifact = head,
        //    //        FeedSource = source,
        //    //        SourceGroup = source.Group,
        //    //        Owner = user,
        //    //        Description = source.Description,
        //    //        Adult = source.Adult,
        //    //        Added = DateTime.UtcNow,
        //    //        ExampleImages = source.ExampleImages,
        //    //        FeedType = source.FeedType,
        //    //        WebUrl = source.WebUrl,
        //    //        FeedTransforms = source.Group.FeedTransforms,
        //    //        Feeds = source.FeedSources.Select((FeedSource s) => CreateFeedFromSource(s, user, source.Params == null ? parms : source.Params.Union(parms, pc).ToList(), ratings)).ToList(),
        //    //    };
        //    //}
        //    //else
        //    //{
        //    //    res = new Feed
        //    //    {
        //    //        Name = source.Name,
        //    //        IsArtifact = head,
        //    //        FeedSource = source,
        //    //        SourceGroup = source.Group,
        //    //        Owner = user,
        //    //        Description = source.Description,
        //    //        Adult = source.Adult,
        //    //        ExampleImages = source.ExampleImages,
        //    //        Added = DateTime.UtcNow,
        //    //        FeedType = source.FeedType,
        //    //        WebUrl = source.WebUrl,
        //    //        FeedTransforms = source.Group.FeedTransforms,
        //    //    };
        //    //}
        //    //// nested url creation allows for url composition from params over multiple levels in semi deterministic fashion.
        //    //// inject the sources params first, then the parents params, then the groups params, then the system params
        //    //res.Url = FormatUrlString(source.Group.FormatUrlString, source.Params == null ? parms.Union(source.Group.Params == null ? new List<Param>() : source.Group.Params.Where(p => p.Value != null), pc).ToList() : source.Params.Union(parms, pc).Union(source.Group.Params == null ? new List<Param>() : source.Group.Params.Where(p => p.Value != null), pc).ToList()).Replace("{culture}", CultureInfo.CurrentCulture.ToString());
        //    //return res;
        //    return new Feed();
        //}

        //public static string FormatUrlString(string fmt, List<Param> prms)
        //{
        //    return fmt.Inject(prms.Distinct(new ParamComparer()).ToDictionary(p => p.Key, p => p.Value));
        //}
    }

    public class ParamComparer : IEqualityComparer<Param>
    {
        public bool Equals(Param x, Param y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x == null || y == null)
                return false;
            if (y.Value == null) return false;
            return x.Key.Equals(y.Key);
        }

        public int GetHashCode(Param obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}
