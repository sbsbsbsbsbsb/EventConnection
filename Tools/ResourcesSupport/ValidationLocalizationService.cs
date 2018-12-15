using Windows.ApplicationModel.Resources;
using JetBrains.Annotations;
using Tools.ValidationEngine.Models;

namespace Tools.ResourcesSupport
{
    public class ValidationLocalizationService : IValidationLocalizationService
    {
        /// <summary>
        /// Gets the localized message from the apps resources and returns it.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetLocalizedMessage([NotNull] string key)
        {
            return ResourceLoader
                .GetForViewIndependentUse()
                .GetString(key);
        }
    }
}