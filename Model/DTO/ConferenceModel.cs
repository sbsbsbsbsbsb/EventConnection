using System.Collections.Generic;
using Newtonsoft.Json;

namespace Model.DTO
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ConferenceModel : BaseModel
    {
        [JsonProperty("dateStart")]
        public bool DateStart { get; set; }

        [JsonProperty("dateEnd")]
        public bool DateEnd { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("brief")]
        public string Brief { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("events")]
        public List<EventModel> Events { get; set; }
    }
}
