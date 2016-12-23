using Newtonsoft.Json;

namespace FaceOneApp.Models
{
    public class Result
    {
        [JsonProperty(PropertyName = "confidence")]
        public float confidence { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public string user_id { get; set; }

        [JsonProperty(PropertyName = "face_token")]
        public string face_token { get; set; }
    }
}