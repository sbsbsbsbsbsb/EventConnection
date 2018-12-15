using Newtonsoft.Json;
using Tools.Attributes;

namespace Model.DTO
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserModel : BaseModel
    {
        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("middlename")]
        public string Middlename { get; set; }

        [JsonProperty("photo")]
        public string Photo { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("socialID")]
        public string SocialId { get; set; }

        [JsonProperty("oauthToken")]
        public string Token { get; set; }

        [JsonProperty("oauthTokenExpireDate")]
        public int? OauthTokenExpireDate { get; set; }

        public string Name => $"{Lastname} {Firstname} {Middlename}";
    }

    public enum UserType
    {
        [Display("UserType_OTHER")]
        OTHER,
        [Display("UserType_IT_COMPANY")]
        IT_COMPANY,
        [Display("UserType_PARTICIPANT")]
        PARTICIPANT,
        [Display("UserType_MEDIA")]
        MEDIA
    }
}
