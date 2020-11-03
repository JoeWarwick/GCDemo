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
    public class SubscriptionsTrigger
    {
        private ISubscriptionsService _subService;

        [FunctionName("Subscribables")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ISubscriptionsService subService, ILogger log)
        {
            this._subService = subService;

            string usr = req.Query["usr"];
            string hash = req.Query["id"];

            var resp = _subService.GetSubscriptions(Guid.Parse(usr), hash);
            var json = JsonConvert.SerializeObject(resp);
            return new OkObjectResult(json);
        }
    }
}
