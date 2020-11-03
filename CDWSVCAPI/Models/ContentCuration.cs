using CDWRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static CDWRepository.CDWSVCModel;

namespace CDWSVCAPI.Models
{
    public class ContentCuration : IContentCuration
    {
        private CDWSVCModel _ctx;

        public ContentCuration(CDWSVCModel ctx)
        {
            _ctx = ctx;
        }

        private List<FeedImage> getImages(string key, object backup)
        {
            try
            {
                var images = _ctx.FeedImages.ToList();
                return images;
            }
            catch (Exception ex)
            {
                //logger.Warn(ex, "CurationImages: Old imageset used as backup");
                return backup as List<FeedImage>;
            }
        }
               
        public List<FeedImage> Images {
            get {
                return getImages(null, null);
            }
        }
        
    }

}