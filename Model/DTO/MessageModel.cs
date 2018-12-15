using Newtonsoft.Json;

namespace Model.DTO
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MessageModel : BaseModel
    {
        [JsonProperty("userFromId")]
        public int UserFromId { get; set; }

        [JsonProperty("userToId")]
        public int UserToId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        public MessageType Type { get; set; }
    }

    public enum MessageType
    {
        Income,
        Outcome
    }
}
