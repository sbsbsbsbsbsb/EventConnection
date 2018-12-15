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
    public class Rating_BestModerator : Tools.ValidationEngine.ViewModel.ViewModel<ReviewStepThreeVM>
    {
        //protected readonly IDataProvider _dataProvider;
        protected readonly INavigationService NavigationService;
        protected IDataProvider DataProvider;
        private ReviewCollector _review;

        [InjectionConstructor]
        public Rating_BestModerator(IDataProvider dataProvider, INavigationService navigationService)
        {
            DataProvider = dataProvider;
            NavigationService = navigationService;

            _review = DataProvider.Review;

            if (_review.StepThree != null)
                Model.GetDataFromModel(_review.StepThree);
        }

        public void GoBack()
        {
            _review.StepOne = null;
            DataProvider.Review = _review;
            //DataProvider.UpdateReview(_review);

            NavigationService.Navigate(nameof(Rating_BestSection), null);
        }

        public void GoForward()
        {
            _review.StepThree = Model.GetDataModel();
            DataProvider.Review = _review;
            //DataProvider.UpdateReview(_review);

            NavigationService.Navigate(nameof(Rating_BestSpeaker), null);
        }
    }
}
