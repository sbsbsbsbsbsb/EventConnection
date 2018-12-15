using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Unity;
using Model.Runtime.Interfaces;
using ViewModels.VM;

namespace ViewModels.Context
{
    public class MenuControl: BaseVM
    {
        private readonly INavigationService _navigationService;
        private bool _isListEnabled =  true;
        private bool _isReviewEnabled;
        private bool _isChatEnabled;

        [InjectionConstructor]
        public MenuControl(IDataProvider dataProvider, INavigationService navigationService)
        {
            _navigationService = navigationService;

            IsReviewEnabled = !dataProvider.Review.IsFinished;

            IsChatEnabled = false; //TODO: implement
        }

        public void GoToList()
        {
            _navigationService.Navigate(nameof(EventsList), null);
        }

        public void GoToReview()
        {
            _navigationService.Navigate(nameof(Rating_Main), null);
        }

        public bool IsListEnabled
        {
            get { return _isListEnabled; }
            set
            {
                _isListEnabled = value;
                OnPropertyChanged(nameof(IsListEnabled));
                OnPropertyChanged(nameof(ListBtnOpacity));
            }
        }

        public double ListBtnOpacity => IsListEnabled ? 1 : 0.5;

        public bool IsReviewEnabled
        {
            get { return _isReviewEnabled; }
            set
            {
                _isReviewEnabled = value;
                OnPropertyChanged(nameof(IsReviewEnabled));
                OnPropertyChanged(nameof(ReviewBtnOpacity));
            }
        }

        public double ReviewBtnOpacity => IsReviewEnabled ? 1 : 0.5;

        public bool IsChatEnabled
        {
            get { return false; }//TODO: _isChatEnabled
            set
            {
                _isChatEnabled = value;
                OnPropertyChanged(nameof(IsChatEnabled));
                OnPropertyChanged(nameof(ChatBtnOpacity));
            }
        }

        public double ChatBtnOpacity => IsChatEnabled ? 1 : 0.5;
    }
}
