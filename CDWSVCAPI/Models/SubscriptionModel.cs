using System;
using System.Collections.Generic;
using CDWRepository;

namespace CDWSVCAPI.Services
{
    public class SubscriptionModel
    {
        public int Id { get; set; }
        public int Rating { get; set; } = 0;
        public string Name { get; set; }
        public string Url { get; set; }
        public string WebUrl { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public virtual DbFeedType FeedType { get; set; }
        public DateTime Added { get; set; } = DateTime.Now;
        public bool Adult { get; set; } = false;
        public virtual List<FeedImage> ExampleImages { get; set; }
    }
}