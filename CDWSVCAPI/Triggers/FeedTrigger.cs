using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CDWSVCAPI.Services;
using Microsoft.AspNetCore.Identity;
using CDWRepository;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace CDWSVCAPI
{
    public class FeedTrigger
    {
        private IFeedService _feedService;
        private readonly ILogger<FeedTrigger> _log;

        public FeedTrigger(IFeedService service, ILogger<FeedTrigger> log)
        {
            _feedService = service;
            _log = log;
        }

        [FunctionName("FeedTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "feed/{usr}/{hash}/{id}")] HttpRequest req)
        {
            string usr = req.Query["usr"];
            string hash = req.Query["hash"];
            string id = req.Query["id"];

            var accept = string.Empty;
            var keyFound = req.Headers.TryGetValue("accept", out StringValues headerValues);
            if (keyFound)
            {
                accept = headerValues.FirstOrDefault();
            }

            var resp = await _feedService.GetFeed(Guid.Parse(usr), hash, int.Parse(id), accept);
            var json = JsonConvert.SerializeObject(resp);
            return new OkObjectResult(json);
        }
    }
}
