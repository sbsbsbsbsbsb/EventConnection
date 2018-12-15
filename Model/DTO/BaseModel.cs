using Newtonsoft.Json;

namespace Model.DTO
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BaseModel
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("dateCreated")]
        public int? DateCreated { get; set; }

        [JsonProperty("lastUpdate")]
        public int? LastUpdate { get; set; }

        [JsonProperty("visible")]
        public bool? Visible { get; set; }
    }
}
