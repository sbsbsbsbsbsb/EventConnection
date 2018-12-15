using Model.DTO;
using Newtonsoft.Json;

namespace Model.DataModel
{
    [JsonObject(MemberSerialization.OptIn)] //for serialisation into local store
    public class ReviewStepTwo
    {
        [JsonProperty]
        public EventModel BestSection { get; set; }
    }
}
