using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using ViewModels.VM;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace EventConnection.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventsList : VisualStateAwarePage
    {
        private readonly ViewModels.Context.EventsList _vm;

        public EventsList()
        {
            InitializeComponent();

            _vm = (ViewModels.Context.EventsList) DataContext;
        }
        
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            EventListBox.ItemsSource = await _vm.GetValues(); //TODO: Move to xaml
        }

        private async void OnClick(object sender, SelectionChangedEventArgs e)
        {
            if(e == null || !e.AddedItems.Any()) return;
            var selectedEvt = (EventListItemVM) e.AddedItems.First();

            if (!selectedEvt.Id.HasValue) throw new ArgumentOutOfRangeException(nameof(e), "Selected event hase not id");

            await _vm.GoToEvent(selectedEvt.Id.Value); //TODO: show progress line
        }
    }
}
