using Newtonsoft.Json;

namespace FaceOneApp.Models
{
    public class Threshold
    {
        [JsonProperty(PropertyName = "1e-3")]
        public float OneEMinus3 { get; set; }

        [JsonProperty(PropertyName = "1e-4")]
        public float OneEMinus4 { get; set; }

        [JsonProperty(PropertyName = "1e-5")]
        public float OneEMinus5 { get; set; }
    }
}