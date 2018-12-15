using Newtonsoft.Json;

namespace Model.DataModel
{
    [JsonObject(MemberSerialization.OptIn)] //for serialisation into local store
    public class ReviewStepFive
    {
        [JsonProperty]
        public string MainReview { get; set; }
    }
}
