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

namespace CDWSVCAPI
{
    public class FeedTrigger
    {
        private IFeedService _feedService;

        [FunctionName("FeedTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "feed/{usr}/{hash}/{id}")] HttpRequest req, IFeedService service,
            ILogger log)
        {
            this._feedService = service;

            string usr = req.Query["usr"];
            string hash = req.Query["hash"];
            string id = req.Query["id"];

            var resp = await _feedService.GetFeed(Guid.Parse(usr), hash, int.Parse(id));
            var json = JsonConvert.SerializeObject(resp);
            return new OkObjectResult(json);
        }
    }
}
