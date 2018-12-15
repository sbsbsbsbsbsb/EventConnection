using JetBrains.Annotations;
using Model.DataModel;
using Model.DataModel.Enum;
using Model.DTO;
using Model.Runtime.Interfaces;
using Tools.ValidationEngine.Models;
using Tools.ValidationEngine.Models.ValidationRules;
using ViewModels.Utils;

namespace ViewModels.VM
{
    public class LoginVM : BaseVM, IViewModel<UserInfo>
    {
        private string _surname;
        private string _firstName;
        private string _patronymic;
        private string _company;
        private UserType _type;

        public string Token { get; set; }

        [ValidateObjectHasValue(LocalizationKey = "FieldRequired",
            ValidationMessageType = typeof (ValidationErrorMessage), FailureMessage = "")]
        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        [ValidateObjectHasValue(LocalizationKey = "FieldRequired",
            ValidationMessageType = typeof (ValidationErrorMessage), FailureMessage = "")]
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        [ValidateObjectHasValue(LocalizationKey = "FieldRequired",
            ValidationMessageType = typeof (ValidationErrorMessage), FailureMessage = "")]
        public string Patronymic
        {
            get { return _patronymic; }
            set
            {
                _patronymic = value;
                OnPropertyChanged(nameof(Patronymic));
            }
        }

        [ValidateObjectHasValue(LocalizationKey = "FieldRequired",
            ValidationMessageType = typeof (ValidationErrorMessage), FailureMessage = "")]
        public string Company
        {
            get { return _company; }
            set
            {
                _company = value;
                OnPropertyChanged(nameof(Company));
            }
        }

        public UserType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public int? OauthTokenExpireDate { get; set; }
        public string SocialId { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }

        public UserInfo GetDataModel()
        {
            return ViewModelConverter.CastToModel(this);
        }

        public void GetInfoFromDataModel([NotNull] UserInfo info)
        {
            ViewModelConverter.CastToView(this, info);
        }
    }
}
