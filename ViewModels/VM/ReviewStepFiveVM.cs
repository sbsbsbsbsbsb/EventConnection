using JetBrains.Annotations;
using Model.DataModel;
using Model.Runtime.Interfaces;
using Tools.ValidationEngine.Models;
using Tools.ValidationEngine.Models.ValidationRules;
using ViewModels.Utils;

namespace ViewModels.VM
{
    public class ReviewStepFiveVM : BaseVM, IViewModel<ReviewStepFive>
    {
        private string _mainReview;

        [ValidateObjectHasValue(LocalizationKey = "FieldRequired",
            ValidationMessageType = typeof (ValidationErrorMessage), FailureMessage = "")]
        public string MainReview
        {
            get { return _mainReview; }
            set
            {
                _mainReview = value;
                OnPropertyChanged(nameof(MainReview));
            }
        }

        public ReviewStepFive GetDataModel()
        {
            return ViewModelConverter.CastToModel(this);
        }

        public void GetDataFromModel([NotNull] ReviewStepFive model)
        {
            ViewModelConverter.CastToView(this, model);
        }
    }
}
