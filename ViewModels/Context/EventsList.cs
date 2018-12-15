using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Unity;
using Model.DTO;
using Model.Runtime.Interfaces;
using ViewModels.VM;

namespace ViewModels.Context
{
    /// <summary>
    /// AutoWired PRISM MVVM Support
    /// </summary>
    public class EventsList : StrongTypeObservableDictionary<string, EventListItemVM>
    {
        //protected readonly IDataProvider _dataProvider;
        protected readonly INavigationService NavigationService;
        protected IBusinessDataProvider ServiceProvider;

        [InjectionConstructor]
        public EventsList(IBusinessDataProvider serviceProvider, INavigationService navigationService)
        {
            ServiceProvider = serviceProvider;
            NavigationService = navigationService;
            Initialization = InitializeAsync();
        }

        public Task Initialization { get; protected set; }
        
        /// <summary>
        /// Init list
        /// </summary>
        /// <returns></returns>
        protected async Task InitializeAsync()
        {
            var events = await ServiceProvider.GetEvents();

            if (events == null) return;

            for (int index = 0; index < events.Count; index++)
            {
                var @event = EventListItemVM.GetVMFromDTO(events[index]);
                Add(index.ToString(), @event);
            }
        }

        public async Task<ICollection<EventListItemVM>> GetValues()
        {
            if(Initialization.Status == TaskStatus.Running || Initialization.Status == TaskStatus.WaitingForActivation || Initialization.Status == TaskStatus.WaitingToRun)
            await Initialization;

            return Values;
        }

        public async Task GoToEvent(int id)
        {
            var @event = await ServiceProvider.GetEvent(id);

            NavigationService.Navigate(nameof(EventDisplay), @event);
        }
    }
}
