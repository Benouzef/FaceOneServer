using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;


namespace FaceOneApp.Models
{
    public class Face
    {
        [JsonProperty(PropertyName = "face_token")]
        public string face_token { get; set; }

        [JsonProperty(PropertyName = "face_rectangle")]
        public FaceRectangle face_rectangle { get; set; }
    }
}