using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CDWSVCAPI.Services;
using System.Net;

namespace CDWSVCAPI
{
    public class SubscriptionsTrigger
    {
        private readonly ISubscriptionsService _subService;
        private readonly ILogger<SubscriptionsTrigger> _log;

        public SubscriptionsTrigger(ISubscriptionsService subservice, ILogger<SubscriptionsTrigger> log)
        {
            _subService = subservice;
            _log = log;
        }

        [FunctionName("Subscriptions")]
        [OpenApiOperation(operationId: "Subscriptions", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "usr", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The User Id parameter")]
        [OpenApiParameter(name: "hash", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The User Client Hash parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(string), Description = "The OK response")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            string usr = req.Query["usr"];
            string hash = req.Query["hash"];

            var resp = _subService.GetSubscriptions(Guid.Parse(usr), hash);
            var json = JsonConvert.SerializeObject(resp);
            return new OkObjectResult(json);
        }
    }
}
