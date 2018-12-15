using Model.DTO;
using Newtonsoft.Json;

namespace Model.DataModel
{
    [JsonObject(MemberSerialization.OptIn)] //for serialisation into local store
    public class ReviewStepFour
    {
        [JsonProperty]
        public EventModel BestSpeakerSection { get; set; }
        [JsonProperty]
        public StaffModel BestSpeaker { get; set; }
    }
}
