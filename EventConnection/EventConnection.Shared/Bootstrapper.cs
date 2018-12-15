using Core.Providers;
#if WINDOWS_PHONE_APP
using EventConnection.Practices;
#endif
using JetBrains.Annotations;
using Microsoft.Practices.Unity;
using Model.Runtime.Interfaces;
using IServiceProvider = Model.Runtime.Interfaces.IServiceProvider;

#if WINDOWS_APP
#endif

namespace EventConnection
{
#if WINDOWS_APP
    public static class Bootstrapper
    {
        public static void RegisterTypes()
        {
            throw new System.NotImplementedException();
        }
    }
#endif
#if WINDOWS_PHONE_APP
    public static class UnityBootstrapper
    {
        public static void RegisterTypes([NotNull] IUnityContainer container)
        {
            container.RegisterType<IServiceProvider, RestServiceProvider>();
            container.RegisterType<IDataProvider, DataStoreProvider>();
            container.RegisterType<IBusinessDataProvider, WPRestProvider>();


            //create instances for singleton registration
            var iServiceProvider = container.Resolve<IServiceProvider>();
            var iDataProvider = container.Resolve<IDataProvider>();
            var iBusinessDataProvider = container.Resolve<IBusinessDataProvider>();

            container.RegisterInstance(iServiceProvider, new ContainerControlledLifetimeManager());
            container.RegisterInstance(iDataProvider, new ContainerControlledLifetimeManager());
            container.RegisterInstance(iBusinessDataProvider, new ContainerControlledLifetimeManager());
            // same as RegisterInstance<interface>(instance)

            //subscriptions
            /* //TODO: restore code
            iServiceProvider.UserUpdated += iDataProvider.UserUpdatedHandler;
            iServiceProvider.EventVoted += iDataProvider.EventVotedHandler;
            iServiceProvider.StaffVoted += iDataProvider.StaffVotedHandler;*/
        }
    }
#endif
}
