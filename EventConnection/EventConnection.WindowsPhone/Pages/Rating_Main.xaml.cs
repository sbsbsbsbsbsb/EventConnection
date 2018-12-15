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
    public sealed partial class Rating_Main : VisualStateAwarePage
    {
        private ViewModels.Context.Rating_Main _vm;

        public Rating_Main()
        {
            InitializeComponent();

            _vm = (ViewModels.Context.Rating_Main) DataContext;

            if (_vm.IsReviewStarted)
                Loaded += (sender, args) =>
                {
                    NavigateToStage();
                };
        }

        private void NavigateToStage()
        {
            if (_vm.IsReviewFinished)
            {
                BlockLayer.Visibility = Visibility.Visible;
                return;
            }

            _vm.NavigateToCurrentStage();
        }

        private void GoBack_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void GoNext_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Rating_Common));//TODO: Use PRISM navigation service
        }
    }
}
