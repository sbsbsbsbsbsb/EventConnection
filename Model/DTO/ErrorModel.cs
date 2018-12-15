using Newtonsoft.Json;

namespace Model.DTO
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ErrorModel
    {
        [JsonProperty("fatal")]
        public bool Fatal { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
