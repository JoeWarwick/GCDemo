using System.Collections.Generic;
using static CDWRepository.CDWSVCModel;

namespace CDWSVCAPI.Models
{
    public interface IContentCuration
    {
        List<FeedImage> Images { get; }
    }
}