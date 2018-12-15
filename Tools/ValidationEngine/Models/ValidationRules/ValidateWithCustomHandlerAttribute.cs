using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tools.ValidationEngine.Models.ValidationRules
{
    /// <summary>
    /// Allows redirecting validation on a property to a specific method.
    /// </summary>
    public class ValidateWithCustomHandlerAttribute : ValidationAttribute
    {
        /// <summary>
        /// Gets or sets the name of the delegate.
        /// </summary>
        /// <value>
        /// The name of the delegate.
        /// </value>
        public string DelegateName { get; set; }

        /// <summary>
        /// Validates the specified property.
        /// </summary>
        /// <param name="property">The property that will its value validated.</param>
        /// <param name="sender">The sender who owns the property.</param>
        /// <returns>
        /// Returns a validation message if validation failed. Otherwise null is returned to indicate a passing validation.
        /// </returns>
        /// <exception cref="System.MissingMethodException"></exception>
        public override IValidationMessage Validate(PropertyInfo property, IValidatable sender)
        {
            if (!CanValidate(sender))
            {
                return null;
            }

            // Set up localization if available.
            PrepareLocalization();

            // Create an instance of our validation message and return it if there is not a delegate specified.
            IValidationMessage validationMessage = Activator.CreateInstance(ValidationMessageType, FailureMessage) as IValidationMessage;
            if (string.IsNullOrEmpty(DelegateName))
            {
                return validationMessage;
            }

            // Find our delegate method.
            IEnumerable<MethodInfo> validationMethods = sender
                .GetType().GetTypeInfo().DeclaredMethods
                .Where(m => m.GetCustomAttributes(typeof(ValidationCustomHandlerDelegate), true).Any());

            MethodInfo validationDelegate = validationMethods.FirstOrDefault(m => m
                    .GetCustomAttributes(typeof(ValidationCustomHandlerDelegate), true)
                    .FirstOrDefault(del => (del as ValidationCustomHandlerDelegate).DelegateName == DelegateName) != null);

            if (validationDelegate == null)
            {
                throw new MissingMemberException(
                    string.Format("Missing {0} validation delegate for {1} instance.", DelegateName, sender.GetType()));
            }

            // Attempt to invoke our delegate method.
            object result = null;
            try
            {
                 result = validationDelegate.Invoke(sender, new object[] { validationMessage, property });

            }
            catch (Exception)
            {
                throw;
            }

            // Return the results of the delegate method.
            if (result != null && result is IValidationMessage)
            {
                return result as IValidationMessage;
            }
            else if (result == null)
            {
                return null;
            }

            return validationMessage;
        }
    }
}
