using Newtonsoft.Json;

namespace StudyGroup.Models
{
    public class Events
    {
        public string id { get; set; }

        [JsonProperty("eventId")]
        public string eventId { get; set; }

        [JsonProperty("eventName")]
        public string eventName { get; set; }

        [JsonProperty("department")]
        public string department { get; set; }

        [JsonProperty("university")]
        public string university { get; set; }

        [JsonProperty("when")]
        public string when { get; set; }

        [JsonProperty("where")]
        public string where { get; set; }
    }
}
