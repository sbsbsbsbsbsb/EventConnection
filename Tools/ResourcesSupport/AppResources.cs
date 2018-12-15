using JetBrains.Annotations;
using Tools.ValidationEngine.Models;

namespace Tools.ResourcesSupport
{
    public static class AppResources
    {
        private static readonly IValidationLocalizationService LocalizationService =
                ValidationLocalizationFactory.CreateService();

        public static string GetResource([NotNull] string resId)
        {
            return LocalizationService.GetLocalizedMessage(resId);
        }
    }
}
