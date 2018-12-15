using System;
using JetBrains.Annotations;
using Model.DataModel;
using Model.DTO;
using ViewModels.VM;

namespace ViewModels.Utils
{
    public static class ViewModelConverter
    {
        #region DataModel -> VM
        public static LoginVM CastToView([NotNull] UserInfo x)
        {
            return new LoginVM
            {
                Company = x.Company,
                FirstName = x.FirstName,
                Patronymic = x.Patronymic,
                Surname = x.Surname,
                //Type = x.Type,
                Token = x.UserToken,
                SocialId = x.SocialId,
                OauthTokenExpireDate = x.UserTokenExpiration,
                Email = x.Email
            };
        }

        public static void CastToView([NotNull] ReviewStepFiveVM reviewStepFiveVM, [NotNull] ReviewStepFive model)
        {
            reviewStepFiveVM.MainReview = model.MainReview;
        }

        public static void CastToView([NotNull] ReviewStepFinalVM reviewStepFinalVM, [NotNull] ReviewFinal model)
        {
            reviewStepFinalVM.DesireForNextEvent = model.DesireForNextEvent;
        }

        public static void CastToView([NotNull] ReviewStepOneVM reviewStepOneVM, [NotNull] ReviewStepOne model)
        {
            reviewStepOneVM.OrganisatorssWorkReview = model.OrganisatorssWorkReview;
            reviewStepOneVM.PlaceReview = model.PlaceReview;
            reviewStepOneVM.RegistrationProcessReview = model.PlaceReview;
            reviewStepOneVM.RelevanceReview = model.RelevanceReview;
        }

        public static void CastToView([NotNull] ReviewStepFourVM reviewStepFourVM, [NotNull] ReviewStepFour model)
        {
            reviewStepFourVM.SelectedSectionId = model.BestSpeakerSection?.Id ?? 0;
            reviewStepFourVM.SelectedSpeakerId = model.BestSpeaker?.Id ?? 0;
        }

        public static void CastToView([NotNull] ReviewStepThreeVM reviewStepThreeVM, [NotNull] ReviewStepThree model)
        {
            reviewStepThreeVM.SelectedSectionId = model.BestModeratorSection?.Id ?? 0;
            reviewStepThreeVM.SelectedModeratorId = model.BestModerator?.Id ?? 0;
        }

        public static void CastToView([NotNull] ReviewStepTwoVM reviewStepTwoVM, [NotNull] ReviewStepTwo model)
        {
            reviewStepTwoVM.SelectedSectionId = model.BestSection?.Id ?? 0;
        }
        #endregion DataModel -> VM

        #region  VM -> DataModel
        public static UserInfo CastToModel([NotNull] LoginVM x)
        {
            return new UserInfo
            {
                Company = x.Company,
                FirstName = x.FirstName,
                Patronymic = x.Patronymic,
                Surname = x.Surname,
                //Type = x.Type,
                UserToken = x.Token,
                SocialId = x.SocialId,
                UserTokenExpiration = x.OauthTokenExpireDate,
                Email = x.Email,
                Photo = x.Photo
            };
        }

        public static ReviewStepOne CastToModel([NotNull] ReviewStepOneVM reviewStepTwoVM)
        {
            return new ReviewStepOne
            {
                OrganisatorssWorkReview = reviewStepTwoVM.OrganisatorssWorkReview,
                PlaceReview = reviewStepTwoVM.PlaceReview,
                RegistrationProcessReview = reviewStepTwoVM.RegistrationProcessReview,
                RelevanceReview = reviewStepTwoVM.RelevanceReview
            };
        }

        public static ReviewStepTwo CastToModel([NotNull] ReviewStepTwoVM vm)
        {
            return new ReviewStepTwo
            {
                BestSection = vm.SelectedSection
            };
        }

        public static ReviewStepThree CastToModel([NotNull] ReviewStepThreeVM vm)
        {
            return new ReviewStepThree
            {
                BestModerator = vm.SelectedModerator,
                BestModeratorSection = vm.SelectedSection
            };
        }

        public static ReviewStepFour CastToModel([NotNull] ReviewStepFourVM vm)
        {
            return new ReviewStepFour
            {
                BestSpeaker = vm.SelectedSpeaker,
                BestSpeakerSection = vm.SelectedSection
            };
        }

        public static ReviewStepFive CastToModel([NotNull] ReviewStepFiveVM vm)
        {
            return new ReviewStepFive
            {
                MainReview = vm.MainReview
            };
        }

        public static ReviewFinal CastToModel([NotNull] ReviewStepFinalVM vm)
        {
            return new ReviewFinal
            {
                DesireForNextEvent = vm.DesireForNextEvent
            };
        }
        #endregion VM -> DataModel

        #region VM -> DTO
        public static UserModel CastToDTO([NotNull] LoginVM x)
        {
            return new UserModel
            {
                CompanyName = x.Company,
                Firstname = x.FirstName,
                Middlename = x.Patronymic,
                Lastname = x.Surname,
                Token = x.Token,
                OauthTokenExpireDate = x.OauthTokenExpireDate,
                SocialId = x.SocialId,
                Email = x.Email
            };
        }
        #endregion VM -> DTO

        #region DTO -> VM
        public static LoginVM CastToview([NotNull] UserModel x)
        {
            
            return new LoginVM
            {
                Company = x.CompanyName,
                FirstName = x.Firstname,
                Patronymic = x.Middlename,
                Surname = x.Lastname,
                Token = x.Token
            };
        }

        public static void CastToView(StaffVM staffVM, StaffModel staffModel)
        {
            staffVM.Id = staffModel.Id;
            staffVM.CountedRating = staffModel.CountedRating;
            staffVM.ReportInfo = staffModel.ReportInfo;
            staffVM.CountedVoters = staffModel.CountedVoters;
            staffVM.DateCreated = staffModel.DateCreated;
            staffVM.Description = staffModel.Description;
            staffVM.Firstname = staffModel.Firstname;
            staffVM.LastUpdate = staffModel.LastUpdate;
            staffVM.Lastname = staffModel.Lastname;
            staffVM.Middlename = staffModel.Middlename;
            staffVM.Photo = staffModel.Photo;
            staffVM.Type = staffModel.Type;
            staffVM.Visible = staffModel.Visible;
        }

        public static EventListItemVM CastToView([NotNull] EventModel model)
        {
            return new EventListItemVM
            {
                DateStart = model.DateStart,
                DateEnd = model.DateEnd,
                Name = model.Name,
                Id = model.Id,
                Brief = model.Brief,
                ConferenceId = model.ConferenceId,
                CountedRating = model.CountedRating,
                CountedVoters = model.CountedVoters,
                DateCreated = model.DateCreated,
                LastUpdate = model.LastUpdate,
                Staffs = model.Staffs,
                Type = model.Type,
                Visible = model.Visible,
                Location = model.Location
            };
        }

        public static void CastToView([NotNull] LoginVM @this, [NotNull] UserInfo info)
        {
            @this.Surname = info.Surname;
            @this.Company = info.Company;
            @this.FirstName = info.FirstName;
            @this.Patronymic = info.Patronymic;
            @this.Token = info.UserToken;
            @this.SocialId = info.SocialId;
            @this.OauthTokenExpireDate = info.UserTokenExpiration;
            @this.Email = info.Email;
            @this.Photo = info.Photo;
        }

        public static StaffVM CastToView([NotNull] StaffModel staffModel)
        {
            return new StaffVM
            {
                Id = staffModel.Id,
                CountedRating = staffModel.CountedRating,
                ReportInfo = staffModel.ReportInfo,
                CountedVoters = staffModel.CountedVoters,
                DateCreated = staffModel.DateCreated,
                Description = staffModel.Description,
                Firstname = staffModel.Firstname,
                LastUpdate = staffModel.LastUpdate,
                Lastname = staffModel.Lastname,
                Middlename = staffModel.Middlename,
                Photo = staffModel.Photo,
                Type = staffModel.Type,
                Visible = staffModel.Visible
            };
        }

        public static void CastToView([NotNull] EventListItemVM model, [NotNull] EventModel eventModel)
        {
            model.Id = eventModel.Id;
            model.CountedRating = eventModel.CountedRating;
            model.Name = eventModel.Name;
            model.CountedVoters = eventModel.CountedVoters;
            model.DateCreated = eventModel.DateCreated;
            model.LastUpdate = eventModel.LastUpdate;
            model.Type = eventModel.Type;
            model.Visible = eventModel.Visible;
            model.Brief = eventModel.Brief;
            model.ConferenceId = eventModel.ConferenceId;
            model.DateEnd = eventModel.DateEnd;
            model.DateStart = eventModel.DateStart;
            model.Location = eventModel.Location;
            model.Staffs = eventModel.Staffs;
        }
        #endregion DTO -> VM
    }
}
