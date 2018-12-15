using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Unity;
using Model.DataModel;
using Model.Runtime.Interfaces;
using ViewModels.VM;

namespace ViewModels.Context
{
    /// <summary>
    /// AutoWired PRISM MVVM Support
    /// </summary>
    public class Rating_BestSpeaker : Tools.ValidationEngine.ViewModel.ViewModel<ReviewStepFourVM>
    {
        //protected readonly IDataProvider _dataProvider;
        protected readonly INavigationService NavigationService;
        protected IDataProvider DataProvider;
        private ReviewCollector _review;

        [InjectionConstructor]
        public Rating_BestSpeaker(IDataProvider dataProvider, INavigationService navigationService)
        {
            DataProvider = dataProvider;
            NavigationService = navigationService;

            _review = DataProvider.Review;

            if (_review.StepFour != null)
                Model.GetDataFromModel(_review.StepFour);
        }

        public void GoBack()
        {
            _review.StepOne = null;
            DataProvider.Review = _review;
            //DataProvider.UpdateReview(_review);

            NavigationService.Navigate(nameof(Rating_BestModerator), null);
        }

        public void GoForward()
        {
            _review.StepFour = Model.GetDataModel();
            DataProvider.Review = _review;
            //DataProvider.UpdateReview(_review);

            NavigationService.Navigate(nameof(Rating_Review), null);
        }
    }
}
