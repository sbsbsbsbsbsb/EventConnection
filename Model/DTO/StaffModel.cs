using Model.DTO.Enum;
using Newtonsoft.Json;

namespace Model.DTO
{
    [JsonObject(MemberSerialization.OptIn)]
    public class StaffModel : BaseModel
    {
        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("middlename")]
        public string Middlename { get; set; }

        [JsonProperty("photo")]
        public string Photo { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("countedRating")]
        public int CountedRating { get; set; }

        [JsonProperty("countedVoters")]
        public int CountedVoters { get; set; }

        [JsonProperty("reportInfo")]
        public string ReportInfo { get; set; }

        [JsonProperty("type")]
        public StaffType Type { get; set; }

        public string Name => $"{Lastname} {Firstname} {Middlename}";
    }
}
