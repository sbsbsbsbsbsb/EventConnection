using System;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Unity;
using Model.Runtime.Interfaces;

namespace ViewModels.VM
{
    public class MainPageVM
    {
        protected readonly IDataProvider DataProvider;
        protected readonly INavigationService NavigationService;

        [InjectionConstructor]
        public MainPageVM(IDataProvider dataProvider, INavigationService navigationService)
        {
            DataProvider = dataProvider;
            NavigationService = navigationService;
        }

        public virtual void InitNavigate()
        {
            throw new NotImplementedException();
        }
    }
}
