Welcome to the Api for the Crowdsourced Desktop Wallpaper service.
Requires https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-3.1.412-windows-x64-installer

Features
     Serverless API ready to scale
     Locally runnable and deployable to Azure
     Cheap to run in Azure on consumption plan and using EFCore to SQLLite db.

This is a work in progress. This work is underway to upgrade from an older monolithic stack system on MSSQL to use as latest; serverless functions as the api, .netcore and EF Core to SQLLite. This will be far cheaper to host on Azure. Expect $270 p/m down to $15 p/m.

To run this, it requires the latest Azure Functions SDK; VS should download it for you on run.
There is a swagger url presented which is compatible with API Manager and API Gateway in Azure as well.
    