using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Tools.ValidationEngine.Models;

namespace Tools.ValidationEngine.Converters
{
    /// <summary>
    /// Converts the existance of errors to color of error of to default if not
    /// </summary>
    public class ValidationCollectionToColorBrush : IValueConverter
    {
        /// <summary>
        /// Converts the first item in a collection of IMessage objects in to a string.
        /// </summary>
        /// <param name="value">A collection of IMessage objects</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns>Returns a string representing the message of the first object in the collection provided.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // The view will provide us with a collection of IMessages.
            if (!(value is IEnumerable<IValidationMessage>))
            {
                throw new ArgumentException("View must provide the converter with a collection of IMessage objects.");
            }

            var collection = (IEnumerable<IValidationMessage>) value;
            return collection.Any() ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
