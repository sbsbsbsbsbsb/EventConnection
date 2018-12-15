using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Unity;
using Model.DTO;
using Model.Runtime;
using Model.Runtime.Interfaces;
using ViewModels.VM;

namespace ViewModels.Context
{
    /// <summary>
    /// AutoWired PRISM MVVM Support
    /// </summary>
    public class EventDisplay: EventListItemVM
    {
        protected IDataProvider DataProvider;
        protected IBusinessDataProvider ServiceProvider;
        protected readonly INavigationService NavigationService;

        private bool _isProcessing = false;

        [InjectionConstructor]
        public EventDisplay(IDataProvider dataProvider, INavigationService navigationService, IBusinessDataProvider serviceProvider)
        {
            DataProvider = dataProvider;
            ServiceProvider = serviceProvider;
            NavigationService = navigationService;

            PropertyChanged += OnVoted;

            PropertyChanged += OnIdSetted;
        }

        private void OnIdSetted(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != nameof(Id)) return;
            if (!Id.HasValue)
                return;
            Voting = DataProvider.GetVoteForEvent(Id.Value);
            if (Voting > 0)
                IsNotVoted = false;
        }

        private async void OnVoted(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != nameof(Voting)) return;
            if(_isProcessing) return;

            _isProcessing = true;
            await Vote(Voting);
            _isProcessing = false;
        }

        public async Task<bool> Vote(int rating)
        {
            if (!Id.HasValue || rating <= 0 || !IsNotVoted)
                return false;

            var result = await ServiceProvider.SetRate(rating, Id.Value);
            IsNotVoted = false;

            return result;
        }

        public async void GoToPerson(int personId)
        {
            if (!Id.HasValue)
                return;

            var personInfo = await ServiceProvider.GetStaff(Id.Value, personId);
            NavigationService.Navigate(nameof(StaffDisplay), new EventStaffNavigationArgs {EventId = Id.Value, Staff = personInfo});
        }
    }
}
