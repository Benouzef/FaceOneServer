using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace FaceOneApp.Models
{
    public class DtoDetect
    {
        [JsonProperty(PropertyName = "request_id")]
        public string request_id { get; set; }

        [JsonProperty(PropertyName = "time_used")]
        public int time_used { get; set; }

        [JsonProperty(PropertyName = "image_id")]
        public string image_id { get; set; }

        [JsonProperty(PropertyName = "faces")]
        public Face[] faces { get; set; }
    }
}