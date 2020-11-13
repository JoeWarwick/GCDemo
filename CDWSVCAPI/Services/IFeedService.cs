using CDWRepository;
using CDWSVCAPI.Caching;
using FeedParsing;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CDWSVCAPI.Services
{
    public interface IFeedService
    {
        CDWSVCModel Model { get; }
        ILogger Logger { get; }
        AutoFeedRefreshCache Cache { get; }
        UserManager<CDWSVCUser> UserManager { get; }

        Task<string> GetFeed(Guid usr, string hash, int id, string accept, string fmt = "");
        Task<IList<Item>> GetEntries(Guid usr, string hash, int id);
        Task<bool> IsPremium(Guid usr, string hash);
        Task<bool> IsValidUser(Guid usr, string hash);
    }
}
