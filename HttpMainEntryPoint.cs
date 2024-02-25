using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.IO;
using website_status_checker.model;

using HttpTriggerAttribute = Microsoft.Azure.Functions.Worker.HttpTriggerAttribute;
using AuthorizationLevel = Microsoft.Azure.Functions.Worker.AuthorizationLevel;

// using System;
// using System.IO;
// using System.Text.Json.Serialization;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Azure.Functions.Worker;
// using Microsoft.Azure.WebJobs;
// using Microsoft.Azure.WebJobs.Extensions.Http;
// using Microsoft.Extensions.Logging;
// using Newtonsoft.Json;

namespace Main
{
    public class HttpMainEntryPoint
    {
        private readonly ILogger<HttpMainEntryPoint> _logger;

        public HttpMainEntryPoint(ILogger<HttpMainEntryPoint> logger)
        {
            _logger = logger;
        }

        [Function("HttpMainEntryPoint")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",
                Route = "validate-urls")] HttpRequest req
                // Route = "validate-urls")] HttpRequest req,
            // FunctionContext executionContext
        )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var body = await new StreamReader(req.Body).ReadToEndAsync();
            WebsiteListRequest websiteList = JsonConvert.DeserializeObject<WebsiteListRequest>(body);

            // Now you can access the list of websites like this
            foreach (var website in websiteList.Urls)
            {
                _logger.LogWarning($"Website Name: {website.Name}, URL: {website.Url}");
            }

            _logger.LogWarning($"Request body urls count: {websiteList.Urls.Count}");

            return new OkObjectResult("Welcome to Azure Functions!");
        }

    }
}
