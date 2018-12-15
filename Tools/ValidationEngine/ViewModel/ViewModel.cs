using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using Tools.ValidationEngine.Models;
using Wukker.Tools;

namespace Tools.ValidationEngine.ViewModel
{
    public class ViewModel<T> : ICommand, INotifyPropertyChanged 
        where T: ValidatableBase, INotifyPropertyChanged, new()
    {
        /// <summary>
        /// Backing field for the view models wrapper around the model validations.
        /// </summary>
        private Dictionary<string, ObservableCollection<IValidationMessage>> _validationMessages;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        public ViewModel()
        {
            Model = ((dynamic) Application.Current).ModelResolve(typeof(T)); //TODO: refactor service location

            //Model = new T();
            ValidationMessages = Model.GetValidationMessages().ConvertValidationMessagesToObservable();

            Model.ValidationChanged += (sender, args) =>
            {
                ValidationMessages[args.ChangedProperty] =
                    new ObservableCollection<IValidationMessage>(args.ValidationMessages);
                OnPropertyChanged("ValidationMessages");
            };
        }

        /// <summary>
        /// Executed when the view model's ability to execute its command has changed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Executed when the view model has had a property value changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the User for the view model.
        /// </summary>
        public T Model { get; set; }

        /// <summary>
        /// Gets a wrapper around the non-observable collection of validation messages stored in our model.
        /// </summary>
        public Dictionary<string, ObservableCollection<IValidationMessage>> ValidationMessages
        {
            get
            {
                return _validationMessages;
            }

            private set
            {
                _validationMessages = value;
                OnPropertyChanged("ValidationMessages");
            }
        }

        /// <summary>
        /// This always returns true.
        /// </summary>
        /// <param name="parameter">The command does not require a parameter to be supplied.</param>
        /// <returns>Always returns true</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Executes the command for this view model
        /// </summary>
        /// <param name="parameter">The command does not require a parameter to be supplied.</param>
        public void Execute(object parameter)
        {
            // Perform validation on the user's built in validation.
            Model.ValidateAll();
            
            // Check if there are any errors.
            // We assign the reference to the view models, which contains an observable collection for the UI.
            if (Model.HasValidationMessages<ValidationErrorMessage>())
            {
                ValidationMessages = Model.GetValidationMessages().ConvertValidationMessagesToObservable();
                return;
            }
            ValidationMessages.AsParallel().ForAll(item => item.Value.Clear());

            // Do stuff.
        }

        /// <summary>
        /// Notifies observers that a specific property has changed.
        /// </summary>
        /// <param name="propertyName">The property that has changed</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler == null)
            {
                return;
            }
            try
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception)
            {
                //TODO: Fix error (double firing value when change property
            }
            OnCanExecuteChanged();
        }

        /// <summary>
        /// Notifies the view that the command needs to be re-evaluated
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            EventHandler handler = CanExecuteChanged;

            handler?.Invoke(this, new EventArgs());
        }
    }
}
