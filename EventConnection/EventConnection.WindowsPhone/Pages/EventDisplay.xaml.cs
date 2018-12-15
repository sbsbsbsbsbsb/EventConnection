using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using Model.DTO;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace EventConnection.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventDisplay : VisualStateAwarePage
    {
        private readonly ViewModels.Context.EventDisplay _vm;

        public EventDisplay()
        {
            InitializeComponent();

            _vm = (ViewModels.Context.EventDisplay) DataContext;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var param = e.Parameter as EventModel;
            if (param != null)
            {
                _vm.GetInfoFromDataModel(param);
            }
        }

        private void OnClick(object sender, SelectionChangedEventArgs e)
        {
            if (e == null || !e.AddedItems.Any()) return;
            var selectedPerson = (StaffModel) e.AddedItems.First();
            if(!selectedPerson.Id.HasValue)
                return;

            _vm.GoToPerson(selectedPerson.Id.Value);
        }
    }
}
