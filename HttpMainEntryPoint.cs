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
using System.Collections.Generic;
using System.Threading.Tasks;

using website_status_checker.model;

using HttpTriggerAttribute = Microsoft.Azure.Functions.Worker.HttpTriggerAttribute;
using AuthorizationLevel = Microsoft.Azure.Functions.Worker.AuthorizationLevel;


namespace website_status_checker
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "validate-urls")] HttpRequest req
        )
        {
            _logger.LogInformation("Request received to validate urls @ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " UTC.");

            // declare a mutable list for the single website responses
            List<SingleWebsiteStatusResponse> websiteListResponse = new List<SingleWebsiteStatusResponse>();

            try
            {
                // read the request body
                var body = await new StreamReader(req.Body).ReadToEndAsync();
                WebsiteListRequest websiteList = JsonConvert.DeserializeObject<WebsiteListRequest>(body);

                // iterate through the website list urls to check the status of the websites
                foreach (var website in websiteList.Urls)
                {


                    // declare a new http client
                    var client = new System.Net.Http.HttpClient();
                    client.DefaultRequestHeaders.Add("User-Agent", "AzureFunctions");

                    // allow sending request to sites that cant be reached
                    client.DefaultRequestHeaders.Add("Accept", "*/*");


                    // declare a string for the website status
                    var websiteStatus = "Up";
                 
                    try {
                        // perform a http request to website.Url
                        // and check the status code and response ok
                        var response = await client.GetAsync(website.Url);


                        // check if response is ok
                        if (response.IsSuccessStatusCode == false)
                        {

                            // check if status code is 401 or 403
                            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                            {
                                websiteStatus = "Unauthorized";
                            }
                            else
                            {
                                websiteStatus = "Down";
                            }   
                        }
                        }
                        catch (Exception ex)
                    {
                        websiteStatus = "Down";
                    }

                    _logger.LogInformation($"Website {website.Name} status: {websiteStatus}");

                    // create a new single website status response
                    SingleWebsiteStatusResponse singleWebsiteStatusResponse = new SingleWebsiteStatusResponse
                    {
                        Name = website.Name,
                        Url = website.Url,
                        Status = websiteStatus
                    };

                    // add the single website status response to the website list response
                    websiteListResponse.Add(singleWebsiteStatusResponse);
                }

            }
            catch (Exception ex)
            {
                // return http response with status code 400
                return (ActionResult) new BadRequestObjectResult( new { message = ex.Message } );
            }

            // return http response with status code 200 with the website list response and the up status of the websites
            return (ActionResult) new OkObjectResult(new WebsiteListResponse { Urls = websiteListResponse } );
           
        }

    }
}
