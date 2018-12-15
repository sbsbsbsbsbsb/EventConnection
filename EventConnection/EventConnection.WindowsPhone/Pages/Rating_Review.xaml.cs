using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using Model.DataModel;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace EventConnection.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Rating_Review : VisualStateAwarePage
    {
        private ViewModels.Context.Rating_Review _vm;


        public Rating_Review()
        {
            InitializeComponent();
            
            _vm = (ViewModels.Context.Rating_Review) DataContext;
        }

        private void GoBack_OnClick(object sender, RoutedEventArgs e)
        {
            _vm.GoBack();
        }

        private void GoNext_OnClick(object sender, RoutedEventArgs e)
        {
            _vm.Model.ValidateAll();
            if (_vm.Model.HasValidationMessages())
                return;

            _vm.GoForward();
        }
    }
}
