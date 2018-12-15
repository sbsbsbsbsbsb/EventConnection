using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using Tools.ValidationEngine.Models;

namespace Wukker.Tools
{
    /// <summary>
    /// Provides method extensions for Dictionary objects.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Converts a dictionary of validation messages from an IValidatableBase instance to an observable dictionary that can be bound
        /// to by the UI.
        /// </summary>
        /// <param name="messages">An existing validation dictionary that needs it's messages observable.</param>
        /// <returns>Returns a new Dictionary that validation messages placed within an ObservableCollection</returns>
        public static Dictionary<string, ObservableCollection<IValidationMessage>> ConvertValidationMessagesToObservable([NotNull] this Dictionary<string, IEnumerable<IValidationMessage>> messages)
        {
            return messages.ToDictionary(pair => pair.Key, pair => new ObservableCollection<IValidationMessage>(pair.Value));
        }
    }
}
