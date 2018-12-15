using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Microsoft.Practices.Prism.Mvvm;
#if WINDOWS_PHONE_APP
using EventConnection.Pages;
using Microsoft.Practices.Unity;
using Facebook.Client;
using EventConnection.Practices;
#endif
using Tools.ResourcesSupport;

// Followed guide for prism mvvm applications here: http://blog.thomaslebrun.net/2014/02/windows-8-1-introduction-to-prism-for-the-windows-runtime
//TODO: implement message provider
namespace EventConnection
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : MvvmAppBase
    {
#if WINDOWS_PHONE_APP
        //private TransitionCollection transitions;
        private readonly IUnityContainer _container = new UnityContainer();
#endif

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            //Suspending += OnSuspending;
#if WINDOWS_PHONE_APP
            ValidationLocalizationFactory.Initialize<ValidationLocalizationService>();
#endif
        }

#if WINDOWS_PHONE_APP //TODO: remove this
        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            var protocolArgs = args as ProtocolActivatedEventArgs;
            LifecycleHelper.FacebookAuthenticationReceived(protocolArgs);
        }
#endif

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
#if WINDOWS_PHONE_APP //TODO: remove this
            NavigationService.Navigate(nameof(MainPage), null);
            UIDispatcher.Initialize();
#endif
            return Task.FromResult<object>(null);
        }

#if WINDOWS_PHONE_APP
        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            UnityBootstrapper.RegisterTypes(_container);

            _container.RegisterInstance(NavigationService);

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {

                var assemblyQualifiedAppType = GetType().GetTypeInfo().AssemblyQualifiedName;
                var pageNameWithParameter = assemblyQualifiedAppType.Replace(GetType().FullName, "ViewModels.Context.{0}");

                //if WINDOWS_PHONE_APP
                pageNameWithParameter = pageNameWithParameter.Replace("EventConnection.WindowsPhone", "ViewModels");
                //ViewModels.Context.MainPage, ViewModels, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
                //endif

                var viewFullName = string.Format(CultureInfo.InvariantCulture, pageNameWithParameter, viewType.Name);
                var viewModelType = Type.GetType(viewFullName);

                return viewModelType;
            });
            return Task.FromResult<object>(null);
        }

        protected override object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public object ModelResolve(Type type)
        {
            return _container.Resolve(type);
        }
#endif

        protected override Type GetPageType(string pageToken)
        {
            var assemblyQualifiedAppType = GetType().GetTypeInfo().AssemblyQualifiedName;
            var pageNameWithParameter = assemblyQualifiedAppType.Replace(GetType().FullName, GetType().Namespace + ".Pages.{0}");
            var viewFullName = string.Format(CultureInfo.InvariantCulture, pageNameWithParameter, pageToken);
            var viewType = Type.GetType(viewFullName);

            return viewType;
        }
    }
}