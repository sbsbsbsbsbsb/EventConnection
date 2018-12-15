using JetBrains.Annotations;
using Model.DataModel;
using Model.DTO;

namespace Model.Utils
{
    public static class ModelConverter
    {
        #region  DTO -> DataModel
        public static UserInfo CastToModel([NotNull] UserModel x)
        {
            return new UserInfo
            {
                Company = x.CompanyName,
                FirstName = x.Firstname,
                Patronymic = x.Middlename,
                Surname = x.Lastname,
                UserToken = x.Token,
                UserTokenExpiration = x.OauthTokenExpireDate,
                SocialId = x.SocialId,
                UserId = x.Id?? 0,
                Photo = x.Photo
            };
        }
        #endregion DTO -> DataModel

        #region  DataModel -> DTO
        public static UserModel CastToDTO([NotNull] UserInfo x)
        {
            return new UserModel
            {
                CompanyName = x.Company,
                Firstname = x.FirstName,
                Middlename = x.Patronymic,
                Lastname = x.Surname,
                Token = x.UserToken,
                Id = x.UserId,
                OauthTokenExpireDate = x.UserTokenExpiration,
                SocialId = x.SocialId,
                Photo = x.Photo
            };
        }

        public static QuestionnaireModel CastToDTO([NotNull] ReviewCollector x)
        {
            return new QuestionnaireModel
            {
                ConferenceId = x.ConferenceId,
                BestModeratorEventId = x.StepThree.BestModeratorSection?.Id ?? 0,
                BestModeratorId = x.StepThree.BestModerator?.Id ?? 0,
                BestSpeakerEventId = x.StepFour.BestSpeakerSection?.Id ?? 0,
                BestSpeakerId = x.StepFour.BestSpeaker?.Id ?? 0,
                OrganizationalScore = x.StepOne.PlaceReview,
                OrganizerWorkScore = x.StepOne.OrganisatorssWorkReview,
                RegistrationProcessScore = x.StepOne.RegistrationProcessReview,
                ThemesRelevanceScore = x.StepOne.RelevanceReview,
                UserId = x.UserId,
                BestEventId = x.StepTwo.BestSection?.Id ?? 0,
                Comment = x.StepFive.MainReview,
                Wishes = x.StepFinal.DesireForNextEvent
            };
        }
        #endregion DataModel -> DTO
    }
}
