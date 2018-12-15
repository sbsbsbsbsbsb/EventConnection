using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using JetBrains.Annotations;

namespace EventConnection.Practices
{
    /// <summary>
    /// http://mikaelkoskinen.net/post/fixing-coredispatcher-invoke-how-to-invoke-method-in-ui-thread-in-windows-8-release-preview
    /// </summary>
    public static class UIDispatcher
    {
        private static CoreDispatcher _dispatcher;

        public static void Initialize()
        {
            _dispatcher = Window.Current.Dispatcher;
        }

        public static void BeginExecute([NotNull] Action action)
        {
            if (_dispatcher.HasThreadAccess)
                action();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            else _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public static void Execute([NotNull] Action action)
        {
            InnerExecute(action).Wait();
        }

        private static async Task InnerExecute([NotNull] Action action)
        {
            if (_dispatcher.HasThreadAccess)
                action();

            else await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }
    }
}
