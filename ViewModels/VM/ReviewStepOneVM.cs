using JetBrains.Annotations;
using Model.DataModel;
using Model.Runtime.Interfaces;
using ViewModels.Utils;

namespace ViewModels.VM
{
    public class ReviewStepOneVM : BaseVM, IViewModel<ReviewStepOne>
    {
        private int _placeReview;
        private int _registrationProcessReview;
        private int _organisatorssWorkReview;
        private int _relevanceReview;

        public int PlaceReview
        {
            get { return _placeReview; }
            set
            {
                _placeReview = value;
                OnPropertyChanged(nameof(PlaceReview));
            }
        }

        public int RegistrationProcessReview
        {
            get { return _registrationProcessReview; }
            set
            {
                _registrationProcessReview = value;
                OnPropertyChanged(nameof(RegistrationProcessReview));
            }
        }

        public int OrganisatorssWorkReview
        {
            get { return _organisatorssWorkReview; }
            set
            {
                _organisatorssWorkReview = value;
                OnPropertyChanged(nameof(OrganisatorssWorkReview));
            }
        }

        public int RelevanceReview
        {
            get { return _relevanceReview; }
            set
            {
                _relevanceReview = value;
                OnPropertyChanged(nameof(RelevanceReview));
            }
        }

        public ReviewStepOne GetDataModel()
        {
            return ViewModelConverter.CastToModel(this);
        }

        public void GetDataFromModel([NotNull] ReviewStepOne model)
        {
            ViewModelConverter.CastToView(this, model);
        }
    }
}
