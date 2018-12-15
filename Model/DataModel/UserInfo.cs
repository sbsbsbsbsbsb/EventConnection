using Model.DataModel.Enum;
using Model.DTO;
using Model.Utils;
using Newtonsoft.Json;

namespace Model.DataModel
{
    [JsonObject(MemberSerialization.OptIn)] //for serialisation into local store
    public class UserInfo
    {
        [JsonProperty]
        public string Surname { get; set; }
        [JsonProperty]
        public string FirstName { get; set; }
        [JsonProperty]
        public string Patronymic { get; set; }
        [JsonProperty]
        public string Company { get; set; }
        [JsonProperty]
        public VisitorType Type { get; set; }
        [JsonProperty]
        public string SocialId { get; set; }
        [JsonProperty]
        public int? UserId { get; set; }
        [JsonProperty]
        public string UserToken { get; set; }
        [JsonProperty]
        public int? UserTokenExpiration { get; set; }
        [JsonProperty]
        public string Email { get; set; }
        [JsonProperty]
        public string Photo { get; set; }

        public static explicit operator UserModel(UserInfo x)
        {
            return ModelConverter.CastToDTO(x);
        }

        public static explicit operator UserInfo(UserModel x)
        {
            return ModelConverter.CastToModel(x);
        }
    }
}
