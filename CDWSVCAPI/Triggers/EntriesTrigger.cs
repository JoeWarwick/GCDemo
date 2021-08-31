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
using System.Linq;
using CDWSVCAPI.Services;
using System.Net;

namespace CDWSVCAPI
{
    public class EntriesTrigger
    {
        private readonly IFeedService _feedService;
        private readonly ILogger<EntriesTrigger> _log;

        public EntriesTrigger(IFeedService feedService, ILogger<EntriesTrigger> log)
        {
            _feedService = feedService;
            _log = log;
        }

        [FunctionName("EntriesTrigger")]
        [OpenApiOperation(operationId: "Entries", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "usr", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The User Id parameter")]
        [OpenApiParameter(name: "hash", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The User Client Hash parameter")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The Entries Id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/xml", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "entries/{usr}/{hash}/{id}")] HttpRequest req)
        {
            string usr = req.Query["usr"];
            string hash = req.Query["hash"];
            string id = req.Query["id"];

            var resp = await _feedService.GetEntries(Guid.Parse(usr), hash, int.Parse(id));
            
            return new OkObjectResult(resp);
        }
    }
}
