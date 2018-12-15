using System.ComponentModel;
using Tools.ValidationEngine.Models;

namespace ViewModels.VM
{
    public class BaseVM : ValidatableBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raised when a property has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that was changed.</param>
        protected void OnPropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
