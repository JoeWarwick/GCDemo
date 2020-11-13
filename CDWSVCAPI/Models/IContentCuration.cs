using System.Collections.Generic;
using CDWRepository;

namespace CDWSVCAPI.Models
{
    public interface IContentCuration
    {
        List<FeedImage> Images { get; }
    }
}