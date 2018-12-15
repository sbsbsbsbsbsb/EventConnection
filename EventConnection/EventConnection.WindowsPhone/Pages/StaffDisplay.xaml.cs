using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using Model.DTO;
using Model.Runtime;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace EventConnection.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StaffDisplay : VisualStateAwarePage
    {
        private ViewModels.Context.StaffDisplay _vm;
        
        public StaffDisplay()
        {
            InitializeComponent();

            _vm = (ViewModels.Context.StaffDisplay) this.DataContext;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var param = e.Parameter as EventStaffNavigationArgs;
            if (param != null && param.EventId.HasValue)
            {
                _vm.CurrentEventId = param.EventId.Value;
                _vm.GetInfoFromDataModel(param.Staff);
            }
        }
    }
}
