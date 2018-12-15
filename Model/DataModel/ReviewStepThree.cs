using Model.DTO;
using Newtonsoft.Json;

namespace Model.DataModel
{
    [JsonObject(MemberSerialization.OptIn)] //for serialisation into local store
    public class ReviewStepThree
    {
        [JsonProperty]
        public EventModel BestModeratorSection { get; set; }
        [JsonProperty]
        public StaffModel BestModerator { get; set; }
    }
}
