using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace website_status_checker.model
{
    public class WebsiteStatusResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }


        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}