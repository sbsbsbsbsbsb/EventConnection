using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Unity;
using Model.Runtime.Interfaces;

namespace ViewModels.Context
{
    /// <summary>
    /// AutoWired PRISM MVVM Support
    /// </summary>
    public class LoginSocial
    {
        //protected readonly IDataProvider _dataProvider;
        protected readonly INavigationService NavigationService;
        protected IBusinessDataProvider ServiceProvider;

        [InjectionConstructor]
        public LoginSocial(IBusinessDataProvider serviceProvider, INavigationService navigationService)
        {
            ServiceProvider = serviceProvider;
            NavigationService = navigationService;
        }

        public async Task<bool> GetIdIfExist(string socialId)
        {
            var id = await ServiceProvider.CheckUser(socialId);
            if (id < 0) return false;

            await ServiceProvider.GetCurrentUser(id); //create user data local

            return true;
        }
    }
}
