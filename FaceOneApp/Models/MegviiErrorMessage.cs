using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace FaceOneApp.Models
{
    public class MegviiErrorMessage
    {
        [JsonProperty(PropertyName = "request_id")]
        public string request_id;

        [JsonProperty(PropertyName = "time_used")]
        public int time_used;

        [JsonProperty(PropertyName = "error_message")]
        public string errorMessage; 
    }
}