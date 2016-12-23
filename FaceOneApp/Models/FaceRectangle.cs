using Newtonsoft.Json;

namespace FaceOneApp.Models
{
    public class FaceRectangle
    {
        [JsonProperty(PropertyName = "width")]
        public int width { get; set; }

        [JsonProperty(PropertyName = "top")]
        public int top { get; set; }

        [JsonProperty(PropertyName = "left")]
        public int left { get; set; }

        [JsonProperty(PropertyName = "height")]
        public int height { get; set; }
    }
}