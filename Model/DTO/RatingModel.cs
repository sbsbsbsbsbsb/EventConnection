using Newtonsoft.Json;

namespace Model.DTO
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RatingModel: BaseModel
    {
        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}