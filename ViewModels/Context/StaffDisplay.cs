using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Model.Runtime.Interfaces;
using ViewModels.VM;

namespace ViewModels.Context
{
    /// <summary>
    /// AutoWired PRISM MVVM Support
    /// </summary>
    public class StaffDisplay: StaffVM
    {
        protected IDataProvider DataProvider;
        protected IBusinessDataProvider ServiceProvider;

        private bool _isProcessing = false;

        [InjectionConstructor]
        public StaffDisplay(IDataProvider dataProvider, IBusinessDataProvider serviceProvider)
        {
            DataProvider = dataProvider;
            ServiceProvider = serviceProvider;

            PropertyChanged += OnVoted;

            PropertyChanged += OnIdSetted;
        }

        private void OnIdSetted(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != nameof(Id)) return;
            if (!Id.HasValue)
                return;
            Voting = DataProvider.GetVoteForStaff(Id.Value);
            if (Voting > 0)
                IsNotVoted = false;
        }

        private async void OnVoted(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != nameof(Voting)) return;
            if (_isProcessing) return;

            _isProcessing = true;
            await Vote(Voting);
            _isProcessing = false;
        }

        public async Task<bool> Vote(int rating)
        {
            if (!Id.HasValue || rating <= 0 || !IsNotVoted)
                return false;

            var result =  await ServiceProvider.SetStaffRate(rating, CurrentEventId, Id.Value);
            IsNotVoted = false;

            return result;
        }
    }
}
