using Newtonsoft.Json;

namespace StudyGroup.Models
{
    public class Whoattended
    {
        public string id { get; set; }

        [JsonProperty("eventId")]
        public string eventId { get; set; }

        [JsonProperty("profile")]
        public string profile { get; set; }
    }
}
