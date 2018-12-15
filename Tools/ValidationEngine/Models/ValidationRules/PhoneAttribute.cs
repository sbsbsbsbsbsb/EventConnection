using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Tools.ValidationEngine.Models.ValidationRules
{
    internal class PhoneAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validates the specified property.
        /// </summary>
        /// <param name="property">The property that will its value validated.</param>
        /// <param name="sender">The sender who owns the property.</param>
        /// <returns>
        /// Returns a validation message if validation failed. Otherwise null is returned to indicate a passing validation.
        /// </returns>
        public override IValidationMessage Validate(PropertyInfo property, IValidatable sender)
        {
            if (!CanValidate(sender))
            {
                return null;
            }

            // Set up localization if available.
            PrepareLocalization();

            var validationMessage = Activator.CreateInstance(ValidationMessageType, FailureMessage) as IValidationMessage;
            var value = property.GetValue(sender, null) ?? string.Empty;

            // While we do convert it to a string below, we want to make sure that the actual Type is a string
            // so that we are not doing a string length comparison check on ToString() of a concrete Type that is not a string.
            IValidationMessage result = null;
            if (value is string)
            {
                var valStr = (string)value;

                Regex regex = new Regex(@"[0-9]{8,20}", RegexOptions.CultureInvariant);
                //^+?[0 - 9]{ 0, 3 }[\\(]{ 0,1} ([0 - 9]){ 3}[\\)]{0,1}[ ]?([^0-1]){1}([0-9]){2}[ ]?[-]?[ ]?([0-9]){4}[ ]*((x){0,1}([0-9]){1,5}){0,1}&

                result = !regex.IsMatch(valStr) ? validationMessage : null;
            }

            return RunInterceptedValidation(sender, property, result);
        }
    }
}
