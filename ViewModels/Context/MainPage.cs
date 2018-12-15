using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Unity;
using Model.Runtime.Interfaces;
using ViewModels.VM;

namespace ViewModels.Context
{
    public class MainPage: MainPageVM
    {
        [InjectionConstructor]
        public MainPage(IDataProvider dataProvider, INavigationService navigationService) : base(dataProvider, navigationService)
        {
        }

        public override void InitNavigate()
        {
            NavigationService.Navigate(DataProvider.CurrentUser == null ? nameof(LoginSocial) : nameof(EventsList), null);
        }
    }
}
