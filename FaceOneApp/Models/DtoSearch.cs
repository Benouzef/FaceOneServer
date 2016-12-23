using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaceOneApp.Models
{
    public class DtoSearch
    {
        [JsonProperty(PropertyName = "request_id")]
        public string request_id { get; set; }

        [JsonProperty(PropertyName = "time_used")]
        public int time_used { get; set; }

        [JsonProperty(PropertyName = "thresholds")]
        public Threshold Threshold { get; set; }


        [JsonProperty(PropertyName = "results")]
        public Result[] results { get; set; }
    }
}