using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EventConnection.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : VisualStateAwarePage
    {
        private ViewModels.Context.MainPage _vm;

        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;

            Loaded += (sender, args) =>
            {
                NavigateIntoApp();
            };
        }

        private void NavigateIntoApp()
        {
            _vm = (ViewModels.Context.MainPage) DataContext;
            _vm.InitNavigate();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
    }
}
