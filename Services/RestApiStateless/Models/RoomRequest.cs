using Newtonsoft.Json;

namespace AffittaCamere.RestApiStateless.Models
{
    public class RoomRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }
    }
}
