using Newtonsoft.Json;

namespace Model.DataModel
{
    [JsonObject(MemberSerialization.OptIn)] //for serialisation into local store
    public class ReviewStepOne
    {
        [JsonProperty]
        public int PlaceReview { get; set; }
        [JsonProperty]
        public int RegistrationProcessReview { get; set; }
        [JsonProperty]
        public int OrganisatorssWorkReview { get; set; }
        [JsonProperty]
        public int RelevanceReview { get; set; }
    }
}
