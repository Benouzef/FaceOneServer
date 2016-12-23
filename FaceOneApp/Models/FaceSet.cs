using Newtonsoft.Json;

namespace FaceOneApp.Models
{
    public class FaceSet
    {
        [JsonProperty(PropertyName = "faceset_token")]
        public string faceset_token;

        [JsonProperty(PropertyName = "outer_id")]
        public string outer_id;

        [JsonProperty(PropertyName = "display_name")]
        public string displayname;

        [JsonProperty(PropertyName = "tags")]
        public string tags;
    }
}