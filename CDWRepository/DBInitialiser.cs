using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace CDWRepository
{
    public class DBInitialiser
    {

        private const string _adminUserName = "admin@CDWSVC.com";
        private const string _baseUserName = "jrwarwick@hotmail.com";
        private const string _baseUserHash = "sdFSSHLFfdllffl";
        private const string _premUserName = "bdeyed3dguy@gmail.com";
        private const string _premUserHash = "GqFbOyAZZkrksdr";

        public async static Task Seed(IServiceProvider sp)
        {
            CDWSVCModel<CDWSVCUser> model = sp.GetService<CDWSVCModel<CDWSVCUser>>();
            await InitializeIdentityForEF(model);
            await InitializeFeedSystem(model);
        }

        public static async Task InitializeIdentityForEF(CDWSVCModel<CDWSVCUser> model)
        {
            var userStore = new UserStore<CDWSVCUser>(model);
            var roleStore = new RoleStore<IdentityRole>(model);
            const string name = _adminUserName;
            const string password = "Sn0rkl3r!";
            const string roleName = "Admin";
            const string baseRoleName = "WallpaperUser";
            const string premRoleName = "PremiumUser";

            var hasher = new PasswordHasher<CDWSVCUser>();

            //Create Role Admin if it does not exist
            IdentityRole role;
            if (!roleStore.Roles.Any(r => r.Name == roleName))
            {
                role = new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = roleName, NormalizedName = roleName.ToUpper() };
                var roleresult = await roleStore.CreateAsync(role);
                role = new IdentityRole { Id = "3c5e174e-3b0e-446f-86af-483d56fd7210", Name = baseRoleName, NormalizedName = baseRoleName.ToLower() };
                roleresult = await roleStore.CreateAsync(role);
                role = new IdentityRole { Id = "4c5e174e-3b0e-446f-86af-483d56fd7210", Name = premRoleName, NormalizedName = premRoleName.ToLower() };
                roleresult = await roleStore.CreateAsync(role);
            }

            if (!userStore.Users.Any(u => u.UserName == roleName.ToLower()))
            {
                var user = new CDWSVCUser
                {
                    Id = "5c5e174e-3b0e-446f-86af-483d56fd7210",
                    UserName = roleName.ToLower(),
                    NormalizedUserName = roleName,
                    Email = name,
                    NormalizedEmail = name,
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString("D"),

                };
                user.PasswordHash = hasher.HashPassword(user, password);
                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, roleName);

                user = new CDWSVCUser
                {
                    Id = "6c5e174e-3b0e-446f-86af-483d56fd7210",
                    UserName = baseRoleName.ToLower(),
                    Email = _baseUserName,
                    NormalizedEmail = _baseUserName,
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    HashStr = _baseUserHash,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };

                user.PasswordHash = hasher.HashPassword(user, password);
                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, baseRoleName);

                user = new CDWSVCUser
                {
                    Id = "7c5e174e-3b0e-446f-86af-483d56fd7210",
                    UserName = _premUserName,
                    Email = _premUserName,
                    HashStr = _premUserHash,
                    NormalizedEmail = _baseUserName,
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };
                user.PasswordHash = hasher.HashPassword(user, password);

                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, baseRoleName);
                await userStore.AddToRoleAsync(user, premRoleName);
            }
        }

        protected static async Task InitializeFeedSystem(CDWSVCModel<CDWSVCUser> context)
        {
            var userStore = new UserStore<CDWSVCUser>(context);
            var adminUser = await userStore.FindByEmailAsync(_adminUserName);
            var baseUser = await userStore.FindByEmailAsync(_baseUserName);
            var premUser = await userStore.FindByEmailAsync(_premUserName);

            var ft1 = context.FeedTypes.Add(new DbFeedType { FeedTypeId = 1, TypeName = "ATOM", Version = "2.0" }).Entity;
            var ft2 = context.FeedTypes.Add(new DbFeedType { FeedTypeId = 2, TypeName = "RSS", Version = "2.0" }).Entity;
            var reddit = context.FeedTransforms.Add(new FeedTransform() { FeedTransformId = 1, Name = "Reddit fixer", Url = "~/XSLT/Reddit.xslt", Shared = true, InputFeedType = ft1, Owner = premUser }).Entity;
            var bing = context.FeedTransforms.Add(new FeedTransform() { FeedTransformId = 2, Name = "Bing fixer", Url = "~/XSLT/Bing.xslt", Shared = true, InputFeedType = ft2, Owner = premUser }).Entity;
            var utf8 = context.FeedTransforms.Add(new FeedTransform() { FeedTransformId = 3, Name = "Flickr fixer", Url = "~/XSLT/CUTF8.xslt", Shared = true, InputFeedType = ft1, Owner = premUser }).Entity;
            var flickr = context.FeedTransforms.Add(new FeedTransform() { FeedTransformId = 4, Name = "Flickr fixer", Url = "~/XSLT/Flickr.xslt", Shared = true, InputFeedType = ft1, Owner = premUser }).Entity;
            var tumblr = context.FeedTransforms.Add(new FeedTransform() { FeedTransformId = 5, Name = "RSS Inner Content", Url = "~/XSLT/InnerSrcRSS.xslt", Shared = true, InputFeedType = ft2, Owner = premUser }).Entity;
            var phone = context.FeedTransforms.Add(new FeedTransform() { FeedTransformId = 6, Name = "Atom Content Link Only", Url = "~/XSLT/AnchorFromContent.xslt", Shared = true, InputFeedType = ft1, Owner = premUser }).Entity;
            var ftg1 = new FeedTag { Tag = "HD" };
            var grp = context.FeedSourceGroups.Add(new FeedSourceGroup
            {
                FeedSourceGroupId = 1,
                Name = "Reddit (safe for work screens)",
                Site = "Reddit",
                Logo = "http://www.lsdi.it/assets/reddit-alien.png",
                BaseUri = "https://www.reddit.com/r/",
                Description = "Reddit is like a popularity contest for best posts",
                ShortName = "Reddit SFW",
                Shared = true,
                Adult = false,
                Rating = 5,
                Owner = adminUser,
                FeedTransforms = new List<FeedSourceGroupFeedTransforms> { { new FeedSourceGroupFeedTransforms { FeedTransform = reddit } } },
                FormatUrlString = "https://www.reddit.com/r/{subreddit}.rss?sort={sort}&top={top}&t={time}",
                Params = new List<FeedSourceGroupParams> {
                    { new FeedSourceGroupParams { Param = new Param { Key = "subreddit", Desc = "Reddit Subreddit(s) to include", Type = "subreddits", Required=true } } },
                    { new FeedSourceGroupParams { Param = new Param { Key = "top", Desc = "Entries to Grab", Type = "positiveInteger", Value = "20" } } },
                    { new FeedSourceGroupParams { Param = new Param { Key="sort", Desc="hot,new,rising,controversial,top,gilded", Type="choice", Value="hot" } } },
                    { new FeedSourceGroupParams { Param = new Param { Key="time", Desc="hour,day,week,month,year,all", Type="choice", Value="day" } } }
                }
            }).Entity;

            var fs1 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 1,
                Name = "Reddit Earthporn",
                ShortName = "earthporn",
                Adult = false,
                ProducesArtifact = true,
                Group = grp,
                Shared = true,
                FeedType = ft1,
                Description = "Pictures of natural landscapes.",
                Params = new List<FeedSourceParams> { new FeedSourceParams { Param = new Param { Key = "subreddit", Value = "earthporn" } } },
                Rating = 5,
                Owner = premUser,
                WebUrl = "https://www.reddit.com/r/earthporn",
                FeedBaseUrl = "https://www.reddit.com/r/earthporn.rss",
                Tags = new List<FeedSourceFeedTags> { new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "Nature" } }, new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "Landscape" } } },
                ExampleImages = new List<FeedSourceExampleImages> { { new FeedSourceExampleImages { ExampleImage = new FeedImage { FeedImageId=1, Url = "http://imgur.com/AmWThvw", Score=12639, Attribution="OurEarthInFocus", Published = new DateTime(2015, 8,23),
                RelLink = "http://i.imgur.com/AmWThvw.jpg", Caption ="I needed a spot to sleep while on a recent road trip and found an epic vantage point above Snoqualmie Pass, WA. Multiple wildfires, and Seattle's light pollution made for a very intense scene!" } } } }
            }).Entity;
            var fs2 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 2,
                Name = "Reddit Space",
                ShortName = "spaceporn",
                Adult = false,
                ProducesArtifact = true,
                Group = grp,
                Shared = true,
                FeedType = ft1,
                Description = "A subreddit devoted to high-quality images of space.",
                Params = new List<FeedSourceParams> { new FeedSourceParams { Param = new Param { Key = "subreddit", Value = "spaceporn" } } },
                Rating = 5,
                Owner = premUser,
                WebUrl = "https://www.reddit.com/r/spaceporn",
                FeedBaseUrl = "https://www.reddit.com/r/spaceporn.rss",
                Tags = new List<FeedSourceFeedTags> {
                    new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "Space" } },
                    new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "NASA" } },
                    new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "SpaceEngine" } }
                },
                ExampleImages = new List<FeedSourceExampleImages> { 
                    new FeedSourceExampleImages { ExampleImage = new FeedImage { Url = "http://www.tsene.com/wp-content/uploads/messenger_orbit.jpg", Score = 6526, Attribution = "ForScale", Published = new DateTime(2013, 2, 10), RelLink = "http://www.tsene.com/wp-content/uploads/messenger_orbit.jpg", Caption = "Clearest pic of Mercury you have ever seen..." } } }
            }).Entity;
            var fs3 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 3,
                Name = "Reddit Cityporn",
                ShortName = "cityporn",
                Adult = false,
                ProducesArtifact = true,
                Group = grp,
                Shared = true,
                FeedType = ft1,
                Description = "A subreddit devoted to cityscapes.",
                Params = new List<FeedSourceParams> { new FeedSourceParams { Param = new Param { Key = "subreddit", Value = "cityporn" } } },
                Rating = 5,
                Owner = premUser,
                WebUrl = "https://www.reddit.com/r/cityporn",
                FeedBaseUrl = "https://www.reddit.com/r/cityporn.rss",
                Tags = new List<FeedSourceFeedTags> {
                    new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "City" } },
                    new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "Architecture" } }, 
                    new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "Cityscape" } }
                },
                ExampleImages = new List<FeedSourceExampleImages> { 
                    new FeedSourceExampleImages { ExampleImage = new FeedImage { Url = "http://i.imgur.com/g33IKI5.jpg", Score = 4722, Attribution = "biwook", Published = new DateTime(2016, 2, 7), RelLink = "http://i.imgur.com/g33IKI5.jpg", Caption = "Highways in Tokyo" } } }
            }).Entity;
            var fsESC = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 4,
                Name = "Reddit Earth, Space, City",
                ShortName = "Reddit E,S,C",
                ProducesArtifact = true,
                Adult = false,
                Group = grp,
                Shared = true,
                FeedType = ft1,
                Rating = 5,
                LoadChildren = false,
                Description = "A mix of Earth, Space and Cities.",
                Params = new List<FeedSourceParams> { new FeedSourceParams { Param = new Param { Key = "subreddit", Value = "earthporn+spaceporn+cityporn" } } },
                FeedSources = new List<FeedSourceChildren> {
                    { new FeedSourceChildren { FeedSourceChild = fs1 } },
                    { new FeedSourceChildren { FeedSourceChild = fs2 } },
                    { new FeedSourceChildren { FeedSourceChild = fs3 } } 
                },
                Owner = premUser,
                WebUrl = "https://www.reddit.com/r/earthporn+spaceporn+cityporn?sort=hot&top=20&t=day",
                FeedBaseUrl = "https://www.reddit.com/r/earthporn+spaceporn+cityporn.rss",
                Tags = new List<FeedSourceFeedTags>().Concat(fs1.Tags).Concat(fs2.Tags).Concat(fs3.Tags).ToList(),
                ExampleImages = new List<FeedSourceExampleImages>().Concat(fs1.ExampleImages).Concat(fs2.ExampleImages).Concat(fs3.ExampleImages).ToList()
            }).Entity;

            //Feeds and FeedSets are the artifacts of the feed building system - users create feeds off of feedsources - a feed is an instance of a feedsource with the params injected hierarchically into the urls from the child run of feedsources.
            context.Subscribables.Add(CreateFromSource(fsESC, baseUser));
            context.Subscribables.Add(CreateFromSource(fsESC, premUser));

            var grpPh = context.FeedSourceGroups.Add(new FeedSourceGroup
            {
                Name = "Reddit Phone Wallpapers",
                Site = "Reddit",
                Logo = "http://www.lsdi.it/assets/reddit-alien.png",
                BaseUri = "https://www.reddit.com/r/",
                Description = "Reddit is like a popularity contest for best posts",
                ShortName = "Reddit SFW",
                Shared = true,
                Adult = false,
                Rating = 5,
                Owner = adminUser,
                FeedTransforms = new List<FeedSourceGroupFeedTransforms> {
                    { new FeedSourceGroupFeedTransforms { FeedTransform = reddit } },
                    { new FeedSourceGroupFeedTransforms { FeedTransform = phone } }
                },
                FormatUrlString = "https://www.reddit.com/r/{subreddit}.rss?sort={sort}&top={top}&t={time}",
                Params = new List<FeedSourceGroupParams> {
                    { new FeedSourceGroupParams { Param = new Param { Key = "subreddit", Desc = "Reddit Subreddit(s) to include", Type = "subreddits", Required=true } } },
                    { new FeedSourceGroupParams { Param = new Param { Key = "top", Desc = "Entries to Grab", Type = "positiveInteger", Value = "20" } } },
                    { new FeedSourceGroupParams { Param = new Param { Key= "sort", Desc="hot,new,rising,controversial,top,gilded", Type="choice", Value="hot" } } },
                    { new FeedSourceGroupParams { Param = new Param { Key= "time", Desc="hour,day,week,month,year,all", Type="choice", Value="week" } } }
                }
            }).Entity;
            var fsPhone = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 5,
                Name = "Mobile Wallpaper",
                ShortName = "Wallpaper",
                Adult = false,
                Group = grpPh,
                Shared = true,
                FeedType = ft1,
                Description = "Reddit Mobile Wallpaper",
                Owner = premUser,
                Tags = new List<FeedSourceFeedTags> { new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "Phone" } } },
                Params = new List<FeedSourceParams> {
                    new FeedSourceParams { Param = new Param { Key = "subreddit", Value = "MobileWallpaper+iWallpaper+VerticalWallpapers+VerticalWallpaper" } },
                    new FeedSourceParams { Param = new Param { Key = "top", Value = "100" } }
                },
            }).Entity;

            context.Subscribables.Add(CreateFromSource(fsPhone, premUser));

            grp = context.FeedSourceGroups.Add(new FeedSourceGroup
            {
                Name = "Reddit Search",
                Logo = "http://www.lsdi.it/assets/reddit-alien.png",
                BaseUri = "https://www.reddit.com/r/",
                Description = "Reddit is like a popularity contest for best posts",
                ShortName = "Reddit Custom",
                Shared = true,
                Adult = false,
                Rating = 3,
                Site = "Reddit",
                Owner = adminUser,
                FeedTransforms = new List<FeedSourceGroupFeedTransforms> {
                    { new FeedSourceGroupFeedTransforms { FeedTransform = reddit } }                  
                },
                FormatUrlString = "https://www.reddit.com/r/{subreddit}/search.rss?q={query}&sort={sort}&top={top}&t={time}&restrict_sr=on",
                Params = new List<FeedSourceGroupParams> {
                    { new FeedSourceGroupParams { Param = new Param { Key = "subreddit", Desc = "Reddit Subreddit(s) to include", Type = "subreddits", Required=true } } },
                    { new FeedSourceGroupParams { Param = new Param { Key = "query", Desc = "Query to search Reddit", Type = "string", Required=true, Value="3840 2160" } } },
                    { new FeedSourceGroupParams { Param = new Param { Key = "top", Desc = "Entries to Grab", Type = "positiveInteger", Value = "20" } } },
                    { new FeedSourceGroupParams { Param = new Param { Key= "sort", Desc="hot,new,rising,conterversial,top,gilded", Type="choice", Value="hot" } } },
                    { new FeedSourceGroupParams { Param = new Param { Key= "time", Desc="hour,day,week,month,year,all", Type="choice", Value="year" } } }
                }
            }).Entity;

            fs1 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 6,
                Name = "Reddit 1920x1200",
                ShortName = "Reddit HD",
                Adult = false,
                Group = grp,
                Shared = true,
                FeedType = ft1,
                Description = "Reddit 1920x1200 Image Search",
                Params = new List<FeedSourceParams> { new FeedSourceParams { Param = new Param { Key = "query", Value = "1920 x 1200" } } },
                Owner = premUser,
                Tags = new List<FeedSourceFeedTags> { new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "1200p" } }, { new FeedSourceFeedTags { FeedTag = ftg1 } } }            
            }).Entity;
            fs2 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 7,
                Name = "Reddit 1600p HD",
                ShortName = "Reddit 1600p",
                Adult = false,
                Group = grp,
                Shared = true,
                FeedType = ft1,
                Description = "Reddit 2560x1600 Image Search",
                Params = new List<FeedSourceParams> { new FeedSourceParams { Param = new Param { Key = "query", Value = "2560 x 1600" } } },
                Owner = premUser,
                Tags = new List<FeedSourceFeedTags> { new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "1600p" } }, { new FeedSourceFeedTags { FeedTag = ftg1 } } }
            }).Entity;
            fs3 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 8,
                Name = "Reddit 1800p HD",
                ShortName = "Reddit 1800p",
                Adult = false,
                Group = grp,
                Shared = true,
                FeedType = ft1,
                Description = "Reddit 3200x1800 Image Search",
                Params = new List<FeedSourceParams> { new FeedSourceParams { Param = new Param { Key = "query", Value = "3200 x 1800" } } },
                Owner = premUser,
                Tags = new List<FeedSourceFeedTags> { new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "1800p" } }, { new FeedSourceFeedTags { FeedTag = ftg1 } } }
            }).Entity;
            var fs4 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 9,
                Name = "Reddit 4K 16:10",
                ShortName = "Reddit 4K",
                Adult = false,
                Group = grp,
                Shared = true,
                FeedType = ft1,
                Description = "Reddit 3840x2400 Image Search",
                Params = new List<FeedSourceParams> { new FeedSourceParams { Param = new Param { Key = "query", Value = "3840 x 2400" } } },
                Owner = premUser,
                Tags = new List<FeedSourceFeedTags> { new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "4k" } }, { new FeedSourceFeedTags { FeedTag = ftg1 } } }
            }).Entity;
            var fs5 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 10,
                Name = "Reddit 16:10 HD",
                ShortName = "Reddit 16:10",
                FeedSources = new List<FeedSourceChildren> {
                    new FeedSourceChildren { FeedSourceChild = fs1 },
                    new FeedSourceChildren { FeedSourceChild = fs2 },
                    new FeedSourceChildren { FeedSourceChild = fs3 },
                    new FeedSourceChildren { FeedSourceChild = fs4 }
                },
                Shared = true,
                FeedType = ft1,
                Owner = baseUser,
                Group = grp,
                Description = "Search for HD images to fit 16:10 resolution",
                Tags = new List<FeedSourceFeedTags> { new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "16:10" } } }
                .Union(fs1.Tags).Union(fs2.Tags).Union(fs3.Tags).Union(fs4.Tags).ToList()
            }).Entity;
            //non artifact producing FeedSource ^^ No required subreddit param

            fs1 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 11,
                Name = "Reddit 1080P",
                ShortName = "Reddit 1080P",
                Adult = false,
                Group = grp,
                Shared = true,
                FeedType = ft1,
                Description = "Reddit 1920x1080 Image Search",
                Params = new List<FeedSourceParams> { new FeedSourceParams { Param = new Param { Key = "query", Value = "1920 x 1080" } } },
                Owner = premUser,
                Tags = new List<FeedSourceFeedTags> { new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "1080p" } }, { new FeedSourceFeedTags { FeedTag = ftg1 } } }
            }).Entity;
            fs2 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 12,
                Name = "Reddit 1440p HD",
                ShortName = "Reddit 1440p",
                Adult = false,
                Group = grp,
                Shared = true,
                FeedType = ft1,
                Description = "Reddit 2560x1440 Image Search",
                Params = new List<FeedSourceParams> { new FeedSourceParams { Param = new Param { Key = "query", Value = "2560 x 1440" } } },
                Owner = premUser,
                Tags = new List<FeedSourceFeedTags> { new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "1440p" } }, { new FeedSourceFeedTags { FeedTag = ftg1 } } }
            }).Entity;
            fs3 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 13,
                Name = "Reddit 4K 16:9",
                ShortName = "Reddit 4K",
                Adult = false,
                Group = grp,
                Shared = true,
                FeedType = ft1,
                Description = "Reddit 3840x2160 Image Search",
                Params = new List<FeedSourceParams> { new FeedSourceParams { Param = new Param { Key = "query", Value = "3840 x 2160" } } },
                Owner = premUser,
                Tags = new List<FeedSourceFeedTags> { new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "2160p" } }, { new FeedSourceFeedTags { FeedTag = ftg1 } } }
            }).Entity;
            fs4 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 14,
                Name = "Reddit 16:9 HD",
                ShortName = "Reddit 16:9",
                ProducesArtifact = true,
                FeedSources = new List<FeedSourceChildren> {
                    new FeedSourceChildren { FeedSourceChild = fs1 },
                    new FeedSourceChildren { FeedSourceChild = fs2 },
                    new FeedSourceChildren { FeedSourceChild = fs3 }
                },
                Shared = true,
                FeedType = ft1,
                Owner = baseUser,
                Group = grp,
                Description = "Search for HD images to fit 16:9 resolution",
                Tags = new List<FeedSourceFeedTags> { new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "16:9" } } }
                    .Union(fs1.Tags).Union(fs2.Tags).Union(fs3.Tags).Union(fs4.Tags).ToList()
            }).Entity;
            var fsHD = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 15,
                Name = "Reddit 16:9|10 HD Earth, Space, City",
                ShortName = "Reddit E,S,P HD",
                Adult = false,
                FeedType = ft1,
                Shared = false,
                Owner = premUser,
                WebUrl = "https://www.reddit.com/r/earthporn+spaceporn+cityporn?sort=hot&top=20&t=day",
                Description = "Reddit 16:9|10 HD Earth, Space, City",
                Group = grp,
                FeedSources = new List<FeedSourceChildren> {
                    new FeedSourceChildren { FeedSourceChild = fsESC },
                    new FeedSourceChildren { FeedSourceChild = fs4 },
                    new FeedSourceChildren { FeedSourceChild = fs5 }
                }
            }).Entity;
            context.Subscribables.Add(CreateFromSource(fsHD, baseUser));
            context.Subscribables.Add(CreateFromSource(fsHD, premUser));

            grp = context.FeedSourceGroups.Add(new FeedSourceGroup
            {
                Name = "Bing!",
                Site = "Bing",
                Logo = "http://www.abondance.com/actualites/wp-content/uploads/2013/11/b-de-bing1.png",
                BaseUri = "http://www.bing.com/HPImageArchive.aspx?format=rss&idx=0&n=15&mkt=en-AU",
                Description = "Bing! Picture of the Day",
                Shared = true,
                Adult = false,
                Rating = 4,
                Owner = adminUser,
                FeedTransforms = new List<FeedSourceGroupFeedTransforms> { { new FeedSourceGroupFeedTransforms { FeedTransform = bing } } },
                FormatUrlString = "http://www.bing.com/HPImageArchive.aspx?format=rss&idx={start}&n={count}&mkt={culture}",
                Params = new List<FeedSourceGroupParams> {
                    new FeedSourceGroupParams { Param = new Param { Key = "start", Desc = "Start Index", Type = "positiveInteger" } },
                    new FeedSourceGroupParams { Param = new Param { Key = "count", Desc = "Entries to Grab", Type = "positiveInteger", Value = "7" } }
                }
            }).Entity;

            fs1 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 16,
                Name = "Bing! Pic of the Day 1-7",
                ShortName = "Bing!",
                Adult = false,
                ProducesArtifact = true,
                Group = grp,
                Shared = true,
                FeedType = ft2,
                Description = "Bing! Picture of the Day",
                Params = new List<FeedSourceParams> { new FeedSourceParams { Param = new Param { Key = "start", Value = "0" } } }
            }).Entity;
            fs2 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 17,
                Name = "Bing! Pic of the Day 8-14",
                ShortName = "Bing!",
                Adult = false,
                ProducesArtifact = true,
                Group = grp,
                Shared = true,
                FeedType = ft2,
                Description = "Bing! Picture of the Day",
                Params = new List<FeedSourceParams> { new FeedSourceParams { Param = new Param { Key = "start", Value = "7" } } }
            }).Entity;
            fs3 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 18,
                Name = "Bing! Pic of the Day 15-21",
                ShortName = "Bing!",
                Adult = false,
                ProducesArtifact = true,
                Group = grp,
                Shared = true,
                FeedType = ft2,
                Description = "Bing! Picture of the Day",
                Params = new List<FeedSourceParams> {
                    new FeedSourceParams { Param = new Param { Key = "start", Value = "14" } },
                    new FeedSourceParams { Param = new Param { Key = "culture", Value = "en-US" } }
                }
            }).Entity;
            fs4 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 19,
                Name = "Bing! Pic of the Day 21-28",
                ShortName = "Bing!",
                Adult = false,
                ProducesArtifact = true,
                Group = grp,
                Shared = true,
                FeedType = ft2,
                Description = "Bing! Picture of the Day",
                Params = new List<FeedSourceParams> {
                    new FeedSourceParams { Param = new Param { Key = "start", Value = "21" } },
                    new FeedSourceParams { Param = new Param { Key = "culture", Value = "en-US" } }
                }
            }).Entity;
            var fsBing = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 20,
                Name = "Bing! Pic of the Day",
                ShortName = "Bing!",
                Adult = false,
                ProducesArtifact = true,
                Group = grp,
                Shared = true,
                FeedType = ft2,
                Rating = 3,
                WebUrl = "http://bing.com/HPImageArchive.aspx?format=rss",
                Owner = premUser,
                FeedBaseUrl = "http://bing.com/HPImageArchive.aspx?format=rss",
                Tags = new List<FeedSourceFeedTags> { 
                    new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "Bing" } } ,
                    new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "Daily" } }  
                },
                FeedSources = new List<FeedSourceChildren> { 
                    new FeedSourceChildren { FeedSourceChild = fs1 }, 
                    new FeedSourceChildren { FeedSourceChild = fs2 } 
                },
                Params = new List<FeedSourceParams> { new FeedSourceParams { Param = new Param { Key = "culture", Value = "en-AU" } } },
                ExampleImages = new List<FeedSourceExampleImages> { new FeedSourceExampleImages { ExampleImage = new FeedImage { Url = "http://www.bing.com/az/hprichbg/rb/LastNightProms_EN-AU6602411502_1920x1080.jpg", Score = -1, Attribution = "chrisstockphotography/Alamy", Published = new DateTime(2016, 10, 2), RelLink = "http://www.bing.com/az/hprichbg/rb/LastNightProms_EN-AU6602411502_1366x768.jpg", Caption = "Acoustic sound panels in the ceiling of the Royal Albert Hall, London" } } }
            }).Entity;

            context.Subscribables.Add(CreateFromSource(fsBing, baseUser));
            context.Subscribables.Add(CreateFromSource(fsBing, premUser));

            grp = context.FeedSourceGroups.Add(new FeedSourceGroup
            {
                Name = "Flickr Groups",
                FeedTransforms = new List<FeedSourceGroupFeedTransforms> { new FeedSourceGroupFeedTransforms { FeedTransform = flickr } },
                Site = "Flickr",
                Logo = "http://farm1.staticflickr.com/1/buddyicons/40961104@N00.jpg?1100187620",
                BaseUri = "http://www.flickr.com/groups",
                Description = "Wallpapers (1024x768 minimum) Pool",
                Shared = true,
                Adult = false,
                Rating = 4,
                Owner = adminUser,
                FormatUrlString = "http://api.flickr.com/services/feeds/groups_pool.gne?id={groupid}&lang={culture}",
                Params = new List<FeedSourceGroupParams> { new FeedSourceGroupParams { Param = new Param { Key = "groupid", Desc = "Flickr Group ID", Type = "string" } } }
            }).Entity;
            fs3 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 21,
                Name = "Flickr Wallpapers 1080p",
                ShortName = "Flickr 1080p",
                ProducesArtifact = true,
                Adult = false,
                Group = grp,
                Shared = true,
                FeedType = ft1,
                Description = "Flickr Wallpapers (1024x768 minimum) Pool",
                Params = new List<FeedSourceParams> { new FeedSourceParams { Param = new Param { Key = "groupid", Value = "40961104@N00" } } },
                Owner = premUser,
                WebUrl = "https://www.flickr.com/groups/wallpapers/pool/",
                FeedBaseUrl = "http://api.flickr.com/services/feeds/groups_pool.gne",
                Rating = 4,
                Tags = new List<FeedSourceFeedTags> { new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "Flickr" } } },
                ExampleImages = new List<FeedSourceExampleImages> { new FeedSourceExampleImages { ExampleImage = new FeedImage { Url = "http://farm9.staticflickr.com/8141/29935708575_d02035b986_b.jpg", Score = -1, Attribution = "arjayempee", Published = new DateTime(2016, 09, 26), RelLink = "http://www.flickr.com/photos/62445171@N00/29935708575/in/pool-40961104@N00", Caption = "Cullen House (2)" } } }
            }).Entity;
            context.Subscribables.Add(CreateFromSource(fs3, baseUser));
            context.Subscribables.Add(CreateFromSource(fs3, premUser));

            grp = context.FeedSourceGroups.Add(new FeedSourceGroup
            {
                Name = "Pinterest",
                FeedTransforms = new List<FeedSourceGroupFeedTransforms> { new FeedSourceGroupFeedTransforms { FeedTransform = utf8 } },
                Site = "Pinterest",
                Logo = "https://upload.wikimedia.org/wikipedia/commons/0/08/Pinterest-logo.png",
                BaseUri = "http://www.pinterest.com/",
                Description = "Pinterest walls are a good place to get good wallpapers",
                Shared = true,
                Adult = false,
                Rating = 3,
                Owner = adminUser,
                FormatUrlString = "http://pinterest.com/{board}/feed.rss",
                Params = new List<FeedSourceGroupParams> { new FeedSourceGroupParams { Param = new Param { Key = "board", Desc = "Pinterest member name or 'popular'", Type = "string", Value = "popular" } } }
            }).Entity;

            grp = context.FeedSourceGroups.Add(new FeedSourceGroup
            {
                Name = "Tumblr",
                FeedTransforms = new List<FeedSourceGroupFeedTransforms> { new FeedSourceGroupFeedTransforms { FeedTransform = tumblr } },
                Site = "Tumblr",
                Logo = "https://assets.tumblr.com/images/logo_page/img_logo_grey_2x.png",
                BaseUri = "http://www.tumblr.com/",
                Description = "Tumblr can be a good place to get good wallpapers",
                Shared = true,
                Adult = false,
                Rating = 3,
                Owner = adminUser,
                FormatUrlString = "http://{board}.tumblr.com/rss",
                Params = new List<FeedSourceGroupParams> { new FeedSourceGroupParams { Param = new Param { Key = "board", Desc = "Tumblr member name", Type = "string", Value = "wallpaper" } } }
            }).Entity;
            fs3 = context.FeedSources.Add(new FeedSource
            {
                FeedSourceId = 22,
                Name = "Tumblr: God Save the Gold",
                ShortName = "GodSaveTheGold",
                ProducesArtifact = true,
                Adult = false,
                Group = grp,
                Shared = true,
                FeedType = ft2,
                Description = "God Save the Gold",
                Owner = premUser,
                WebUrl = "http://godsavethegold.tumblr.com/",
                Rating = 4,
                Tags = new List<FeedSourceFeedTags> { new FeedSourceFeedTags { FeedTag = new FeedTag { Tag = "Tumblr" } } },
                Params = new List<FeedSourceParams> { new FeedSourceParams { Param = new Param { Key = "board", Value = "godsavethegold" } } }
            }).Entity;
            context.Subscribables.Add(CreateFromSource(fs3, baseUser));
            context.Subscribables.Add(CreateFromSource(fs3, premUser));

            await context.SaveChangesAsync();
        }

        //A feed subscription is an artifact that is generated from a feed source. It is stamped out like a ticket.
        //This code will perhaps need to operate on the client side too 
        public static Subscribable CreateFromSource(FeedSource source, CDWSVCUser user)
        {
            var avg = new List<int>();
            if (source.FeedSources == null) source.FeedSources = new List<FeedSourceChildren>();
            var parms = source.Params == null ? 
                 source.FeedSources.Select(sc => sc.FeedSourceChild).Where(f => f.Params != null).SelectMany(f => f.Params).ToList() 
                : source.Params.Union(source.FeedSources.Select(sc => sc.FeedSourceChild).Where(f => f.Params != null).SelectMany(f => f.Params)).ToList();
            var res = CreateFeedFromSource(source, user, parms.Select(p => p.Param).ToList(), avg, true);
            var rates = avg.Where(v => v > 0);
            res.Rating = rates.Count() == 0 ? 2 : Convert.ToInt16(rates.Average());
            return res;
        }

        public static Subscribable CreateFeedFromSource(FeedSource source, CDWSVCUser user, List<Param> parms, List<int> ratings, bool head = false)
        {
            Subscribable res;
            var pc = new ParamComparer();
            if (source.Rating > 0)
            {
                ratings.Add(source.Rating);
            }
            if (source.FeedSources != null && source.FeedSources.Count() > 1)
            {
                res = new FeedSet
                {
                    Name = source.Name,
                    IsArtifact = head,
                    FeedSource = source,
                    SourceGroup = source.Group,
                    Owner = user,
                    Description = source.Description,
                    Adult = source.Adult,
                    Added = DateTime.UtcNow,
                    ExampleImages = source.ExampleImages.Select(e => new SubscribableExampleImages
                    {
                        ExampleImage = e.ExampleImage
                    })
                    .ToList(),
                    FeedType = source.FeedType,
                    WebUrl = source.WebUrl,
                    FeedTransforms = source.Group.FeedTransforms.Select(e => new SubscribableFeedTransforms
                    {
                        FeedTransform = e.FeedTransform
                    }).ToList(),
                    Feeds = source.FeedSources.Select(s =>
                        CreateFeedFromSource(s.FeedSourceChild, user, source.Params == null ? parms : source.Params.Select(p => p.Param).Union(parms, pc)
                        .ToList(), ratings))
                    .ToList()
                };
            }
            else
            {
                res = new Feed
                {
                    Name = source.Name,
                    IsArtifact = head,
                    FeedSource = source,
                    SourceGroup = source.Group,
                    Owner = user,
                    Description = source.Description,
                    Adult = source.Adult,
                    ExampleImages = source.ExampleImages.Select(e => new SubscribableExampleImages
                    {
                        ExampleImage = e.ExampleImage
                    })
                    .ToList(),
                    Added = DateTime.UtcNow,
                    FeedType = source.FeedType,
                    WebUrl = source.WebUrl,
                    FeedTransforms = source.Group.FeedTransforms.Select(e => new SubscribableFeedTransforms
                    {
                        FeedTransform = e.FeedTransform
                    }).ToList()
                };
            }
            // nested url creation allows for url composition from params over multiple levels in semi deterministic fashion.
            // inject the sources params first, then the parents params, then the groups params, then the system params
            var sourceParams = source.Params?.Where(p => p.Param.Value != null).Select(p => p.Param).ToList() ?? new List<Param>();
            var groupParams = source.Group.Params?.Where(p => p.Param.Value != null).Select(p => p.Param).ToList() ?? new List<Param>();
             
            res.Url = FormatUrlString(source.Group.FormatUrlString, parms.Union(sourceParams).Union(groupParams).ToList()).Replace("{culture}", CultureInfo.CurrentCulture.ToString());
            return res;
        }

        public static string FormatUrlString(string fmt, List<Param> prms)
        {
            foreach (var param in prms)
            {
                fmt = fmt.Replace($"{{{param.Key}}}", param.Value);
            }
            return fmt;
        }
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
