using Newtonsoft.Json;

namespace Model.DTO
{
    [JsonObject(MemberSerialization.OptIn)]
    public class QuestionnaireModel : BaseModel
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("conferenceId")]
        public int ConferenceId { get; set; }
        [JsonProperty("organizationalScore")]
        public int OrganizationalScore { get; set; }
        [JsonProperty("registrationProcessScore")]
        public int RegistrationProcessScore { get; set; }
        [JsonProperty("organizerWorkScore")]
        public int OrganizerWorkScore { get; set; }
        [JsonProperty("themesRelevanceScore")]
        public int ThemesRelevanceScore { get; set; }
        [JsonProperty("bestModeratorEventId")]
        public int BestModeratorEventId { get; set; }
        [JsonProperty("bestModeratorId")]
        public int BestModeratorId { get; set; }
        [JsonProperty("bestSpeakerEventId")]
        public int BestSpeakerEventId { get; set; }
        [JsonProperty("bestSpeakerId")]
        public int BestSpeakerId { get; set; }
        [JsonProperty("bestEventId")]
        public int BestEventId { get; set; }
        [JsonProperty("wishes")]
        public string Wishes { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}
