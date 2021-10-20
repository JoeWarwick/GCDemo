using GCDAPI.Services;
using GCDRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace GCDAPI
{
    public class ToDoTrigger
    {
        private readonly IToDoService _todoService;
        private readonly ILogger<ToDoTrigger> _log;

        public ToDoTrigger(IToDoService todoService, ILogger<ToDoTrigger> log)
        {
            _todoService = todoService;
            _log = log;
        }

        [FunctionName("ToDos")]
        [OpenApiOperation(operationId: "Get ToDos", tags: new[] { "name" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IList<ToDoItem>), Description = "The OK response")]
        public ActionResult<IList<ToDoItem>> GETTODOS(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos")] HttpRequest req)
        {
            
            var resp = _todoService.GetToDos();
            
            return new OkObjectResult(resp);
        }

        [FunctionName("AddToDo")]
        [OpenApiRequestBody("application/json", typeof(ToDoItem))]
        [OpenApiOperation(operationId: "Add ToDo", tags: new[] { "name" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ToDoItem), Description = "The OK response")]
        public async Task<ActionResult<ToDoItem>> ADDTODO(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ToDoItem data = JsonConvert.DeserializeObject<ToDoItem>(requestBody);
            await _todoService.AddToDo(data);

            return new OkObjectResult(data);
        }

        [FunctionName("PutToDo")]
        [OpenApiRequestBody("application/json", typeof(ToDoItem))]
        [OpenApiOperation(operationId: "Set ToDo", tags: new[] { "name" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ToDoItem), Description = "The OK response")]
        public async Task<ActionResult<ToDoItem>> PUTTODO(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ToDoItem data = JsonConvert.DeserializeObject<ToDoItem>(requestBody);
            var res = await _todoService.SetToDo(data);

            return new OkObjectResult(res);
        }

        [FunctionName("DeleteToDo")]
        [OpenApiOperation(operationId: "Delete ToDo", tags: new[] { "name" })]
        [OpenApiParameter("Id", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(bool), Description = "The OK response")]
        public ActionResult<IList<ToDoItem>> DELTODO(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todo")] int id)
        {

            var resp = _todoService.DeleteToDo(id);

            return new OkObjectResult(resp);
        }
    }
}
