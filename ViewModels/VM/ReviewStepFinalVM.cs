using JetBrains.Annotations;
using Model.DataModel;
using Model.Runtime.Interfaces;
using Tools.ValidationEngine.Models;
using Tools.ValidationEngine.Models.ValidationRules;
using ViewModels.Utils;

namespace ViewModels.VM
{
    public class ReviewStepFinalVM : BaseVM, IViewModel<ReviewFinal>
    {
        private string _desireForNextEvent;

        [ValidateObjectHasValue(LocalizationKey = "FieldRequired",
            ValidationMessageType = typeof (ValidationErrorMessage), FailureMessage = "")]
        public string DesireForNextEvent
        {
            get { return _desireForNextEvent; }
            set
            {
                _desireForNextEvent = value;
                OnPropertyChanged(nameof(DesireForNextEvent));
            }
        }

        public ReviewFinal GetDataModel()
        {
            return ViewModelConverter.CastToModel(this);
        }

        public void GetDataFromModel([NotNull] ReviewFinal model)
        {
            ViewModelConverter.CastToView(this, model);
        }
    }
}
