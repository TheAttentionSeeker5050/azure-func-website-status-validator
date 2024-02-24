using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

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
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "/")] HttpRequest req,
            FunctionContext executionContext
        )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
