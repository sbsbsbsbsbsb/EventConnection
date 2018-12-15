using System.Threading.Tasks;
using Core.Providers;
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
    public class LoginPageForm : Tools.ValidationEngine.ViewModel.ViewModel<LoginVM>
    {
        //protected readonly IDataProvider _dataProvider;
        protected readonly INavigationService NavigationService;
        protected IBusinessDataProvider ServiceProvider;

        [InjectionConstructor]
        public LoginPageForm(IBusinessDataProvider serviceProvider, INavigationService navigationService)
        {
            ServiceProvider = serviceProvider;
            NavigationService = navigationService;
        }

        public async Task CreateUser()
        {
            var user = (UserModel) Model.GetDataModel();
            var result = await ServiceProvider.AddUser(user);

            if (result >= 0)
            {
                var resultFinal = await ServiceProvider.AddUserToConf(user.Token, ConferenceManager.GetConferenceId(), Model.Type);
                if (resultFinal)
                    NavigationService.Navigate(nameof(EventsList), null);
            }
        }
    }
}
