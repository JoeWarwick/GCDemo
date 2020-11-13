using CDWSVCAPI.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CDWRepository;

namespace CDWSVCAPI.Caching
{
    public class AutoImageRefreshCache : AutoRefreshCache<Tuple<string, string>, List<FeedImage>>
    {
        private IContentCuration _curation;
        public AutoImageRefreshCache(IContentCuration curation, ILogger logger) 
            : base(interval: TimeSpan.FromMinutes(120), logger) 
        {
            this._curation = curation;
        }

        protected override List<FeedImage> Load(Tuple<string, string> key)
        {
            switch (key.Item1)
            {
                case "NoShowImages":
                    return _curation.Images.Where(fi => fi.Url.StartsWith(key.Item2) && fi.DoNotShow).ToList();
                case "NoShowMsgs":
                    return _curation.Images.Where(fi => fi.Url.StartsWith(key.Item2) && fi.Watermarking == bool.FalseString).ToList();
                default:
                    return new List<FeedImage>();
            }            
        }
    }
}