using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using JetBrains.Annotations;

namespace Tools.Extentions
{
    public static class ProgressBarExtention
    {

        public static void SetProgressIndicator([NotNull] this ProgressBar bar, bool value)
        {
            if (value)
            {
                bar.IsEnabled = true;
                bar.Visibility = Visibility.Visible;
            }
            else
            {
                bar.IsEnabled = false;
                bar.Visibility = Visibility.Collapsed;
            }
        }
    }
}
