using Newtonsoft.Json;

namespace StudyGroup.Models
{
    public class Users
    {
        public string id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("age")]
        public string Age { get; set; }

        [JsonProperty("profilepicture")]
        public string ProfilePicture { get; set; }
        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("university")]
        public string University { get; set; }

        [JsonProperty("profile")]
        public string Profile { get; set; }
    }
}
