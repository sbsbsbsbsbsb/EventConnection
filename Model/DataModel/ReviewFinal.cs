using Newtonsoft.Json;

namespace Model.DataModel
{
    [JsonObject(MemberSerialization.OptIn)] //for serialisation into local store
    public class ReviewFinal
    {
        [JsonProperty]
        public string DesireForNextEvent { get; set; }
    }
}
