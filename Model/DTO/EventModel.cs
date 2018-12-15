using System.Collections.Generic;
using Model.DTO.Enum;
using Newtonsoft.Json;

namespace Model.DTO
{
    [JsonObject(MemberSerialization.OptIn)]
    public class EventModel: BaseModel
    {
        [JsonProperty("conferenceId")]
        public int ConferenceId { get; set; }

        [JsonProperty("dateStart")]
        public int DateStart { get; set; }

        [JsonProperty("dateEnd")]
        public int DateEnd { get; set; }

        [JsonProperty("type")]
        public EventType Type { get; set; }

        [JsonProperty("countedRating")]
        public int CountedRating { get; set; }

        [JsonProperty("countedVoters")]
        public int CountedVoters { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("brief")]
        public string Brief { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("staffs")]
        public List<StaffModel> Staffs { get; set; }
    }
}
