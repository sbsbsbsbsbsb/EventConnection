using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;
using Tools.ValidationEngine.Models;

namespace Tools.ValidationEngine.Converters
{
    public class IMessageCollectionToStringCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is IEnumerable<IValidationMessage>))
            {
                return new List<string>();
            }

            List<string> lst = (value as IEnumerable<IValidationMessage>).Select(m => m.Message).ToList();
            return lst;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
