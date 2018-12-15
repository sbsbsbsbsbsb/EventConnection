using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Unity;
using Model.DataModel;
using Model.DTO;
using Model.Runtime.Interfaces;
using ViewModels.VM;

namespace ViewModels.Context
{
    /// <summary>
    /// AutoWired PRISM MVVM Support
    /// </summary>
    public class Rating_Final : Tools.ValidationEngine.ViewModel.ViewModel<ReviewStepFinalVM>
    {
        //protected readonly IDataProvider _dataProvider;
        protected readonly INavigationService NavigationService;
        protected IDataProvider DataProvider;
        protected IBusinessDataProvider ServiceProvider;
        private ReviewCollector _review;

        [InjectionConstructor]
        public Rating_Final(IDataProvider dataProvider, INavigationService navigationService, IBusinessDataProvider serviceProvider)
        {
            DataProvider = dataProvider;
            NavigationService = navigationService;
            ServiceProvider = serviceProvider;

            _review = DataProvider.Review;

            if (_review.StepFinal != null)
                Model.GetDataFromModel(_review.StepFinal);
        }

        public void GoBack()
        {
            _review.StepOne = null;
            DataProvider.Review = _review;
            //DataProvider.UpdateReview(_review);

            NavigationService.Navigate(nameof(Rating_Review), null);
        }

        public async void GoForward()
        {
            _review.StepFinal = Model.GetDataModel();
            DataProvider.Review = _review;
            //DataProvider.UpdateReview(_review);

            var result = await ServiceProvider.AddReview(DataProvider.CurrentUser.UserId.Value, (QuestionnaireModel)_review);
            if (result < 0) return;

            _review.IsFinished = true;
            DataProvider.Review = _review;
            //DataProvider.UpdateReview(_review);

            NavigationService.Navigate(nameof(EventsList), null);
        }
    }
}
