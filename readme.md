GCDemo

Welcome to the Api for the Todo service.
Requires https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-3.1.412-windows-x64-installer to run locally.

Features
     Serverless API ready to scale
     Locally runnable and deployable to Azure
     Cheap to run in Azure on consumption plan and using EFCore to SQLLite db.

To run the API locally, it requires the latest Azure Functions SDK; VS should download it for you on run.
There is a swagger url presented which is compatible with API Manager and API Gateway in Azure as well.

The API is deployed to Azure so there is not much need to run it locally. The react app will communicate with the azure api.

The react app may be run locally from the Client/gcdclient folder. Run 'npm start' from there to do so.

I am deploying the client to an Azure linux node host https://gcdclient.azurewebsites.net/
Perculiar errors on deployment are occurring as yet. Still investigating. You may run the client locally to view it.


    
Here are the endpoints:

Functions:

        ToDos: [GET] http://localhost:7071/api/todos
               [GET] https://gcdapiapi.azure-api.net/todo/todos
        
        AddToDo: [POST] http://localhost:7071/api/todo
                 [POST] https://gcdapiapi.azure-api.net/todo/todo

        DeleteToDo: [DELETE] http://localhost:7071/api/todo/{id:int}
                    [DELETE] https://gcdapiapi.azure-api.net/todo/todo

        PutToDo: [PUT] http://localhost:7071/api/todo
                 [PUT] https://gcdapiapi.azure-api.net/todo/todo

        RenderOpenApiDocument: [GET] http://localhost:7071/api/openapi/2.json

        RenderSwaggerDocument: [GET] http://localhost:7071/api/swagger.json

        RenderSwaggerUI: [GET] http://localhost:7071/api/swagger/ui

        
