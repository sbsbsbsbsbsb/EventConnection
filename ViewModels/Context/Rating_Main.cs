using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Unity;
using Model.DataModel;
using Model.Runtime.Interfaces;

namespace ViewModels.Context
{
    public class Rating_Main
    {
        public bool IsReviewStarted => _review.ReviewProgress != 0;
        public bool IsReviewFinished => _review.IsFinished;

        //protected readonly IDataProvider _dataProvider;
        protected readonly INavigationService NavigationService;
        protected IDataProvider DataProvider;
        private ReviewCollector _review;

        [InjectionConstructor]
        public Rating_Main(IDataProvider dataProvider, INavigationService navigationService)
        {
            DataProvider = dataProvider;
            NavigationService = navigationService;

            _review = DataProvider.Review;
        }

        public void NavigateToCurrentStage()
        {
            if (_review.ReviewProgress != 0)
            {
                switch (_review.ReviewProgress)
                {
                    case 1:
                        NavigationService.Navigate(nameof(Rating_BestSection), null);
                        break;
                    case 2:
                        NavigationService.Navigate(nameof(Rating_BestModerator), null);
                        break;
                    case 3:
                        NavigationService.Navigate(nameof(Rating_BestSpeaker), null);
                        break;
                    case 4:
                        NavigationService.Navigate(nameof(Rating_Review), null);
                        break;
                    case 5:
                    case 6:
                        NavigationService.Navigate(nameof(Rating_Final), null);
                        break;
                }
            }
        }
    }
}
