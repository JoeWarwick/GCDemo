using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using CDWSVCAPI.Services;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Net;

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
        [OpenApiOperation(operationId: "Feed", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "usr", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The User Id parameter")]
        [OpenApiParameter(name: "hash", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The User Client Hash parameter")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The Feed Id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/rss+xml", bodyType: typeof(string), Description = "The RSS Feed response")]
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
            return new OkObjectResult(resp);
        }
    }
}
