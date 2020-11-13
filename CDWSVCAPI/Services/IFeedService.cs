using CDWRepository;
using CDWSVCAPI.Caching;
using FeedParsing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CDWSVCAPI.Services
{
    public interface IFeedService
    {
        CDWSVCModel Model { get; }
        ILogger Logger { get; }
        AutoFeedRefreshCache Cache { get; }

        public Task<string> GetFeed(Guid usr, string hash, int id, string fmt = "");

        public IList<Item> GetEntries(Guid usr, string hash, int id);
    }
}
