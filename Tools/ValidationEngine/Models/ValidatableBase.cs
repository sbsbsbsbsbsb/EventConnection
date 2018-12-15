//-----------------------------------------------------------------------
// <copyright file="ValidatableBase.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Tools.ValidationEngine.Models.ValidationRules;

namespace Tools.ValidationEngine.Models
{
    /// <summary>
    /// An implementation of IValidatable that provides a base class for validating instances with property attributes implementing IValidationRule.
    /// </summary>
    public class ValidatableBase : IValidatable
    {
        /// <summary>
        /// The property validation reflection cache
        /// </summary>
        private static readonly Dictionary<Type, Dictionary<PropertyInfo, IEnumerable<IValidationRule>>> PropertyValidationCache
            = new Dictionary<Type, Dictionary<PropertyInfo, IEnumerable<IValidationRule>>>();

        /// <summary>
        /// The ValidationMessages backing field.
        /// </summary>
        protected Dictionary<string, ICollection<IValidationMessage>> ValidationMessages;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatableBase" /> class.
        /// </summary>
        public ValidatableBase()
        {
            ValidationMessages = new Dictionary<string, ICollection<IValidationMessage>>();
            SetupValidation();
        }

        /// <summary>
        /// Occurs when the instances validation state has changed.
        /// </summary>
        public event EventHandler<ValidationChangedEventArgs> ValidationChanged;

        /// <summary>
        /// Adds a validation message to the instance.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="property">The property.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">property;You must supply a property name when adding a new validation message to this instance.</exception>
        public void AddValidationMessage(IValidationMessage message, string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentOutOfRangeException("property", "You must supply a property name when adding a new validation message to this instance.");
            }

            // If the key does not exist, then we create one.
            if (!ValidationMessages.ContainsKey(property))
            {
                ValidationMessages[property] = new List<IValidationMessage>();
            }

            if (ValidationMessages[property].Any(msg => msg.Message.Equals(message.Message) || msg == message))
            {
                return;
            }

            ValidationMessages[property].Add(message);
			
            // Publish our new validation collection for this property.
            OnValidationChanged(new ValidationChangedEventArgs(property, ValidationMessages[property]));
        }

        /// <summary>
        /// Removes all of the instances validation messages.
        /// </summary>
        public void RemoveValidationMessages()
        {
            foreach (KeyValuePair<string, ICollection<IValidationMessage>> pair in ValidationMessages)
            {
                pair.Value.Clear();

                // Publish our new validation collection for this property.
                OnValidationChanged(new ValidationChangedEventArgs(pair.Key, ValidationMessages[pair.Key]));
            }
        }

        /// <summary>
        /// Removes all of the instances validation messages for the given property.
        /// </summary>
        /// <param name="property">The property.</param>
        public void RemoveValidationMessages(string property)
        {
            if (!string.IsNullOrEmpty(property) && ValidationMessages.ContainsKey(property))
            {
                // Remove all validation messages for the property if a message isn't specified.
                ValidationMessages[property].Clear();
                OnValidationChanged(new ValidationChangedEventArgs(property, ValidationMessages[property]));
            }
        }

        /// <summary>
        /// Removes the given validation message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="property">The property.</param>
        public void RemoveValidationMessage(IValidationMessage message, string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                return;
            }

            if (!ValidationMessages.ContainsKey(property))
            {
                return;
            }

            if (ValidationMessages[property].Any(msg => msg == message || msg.Message.Equals(message.Message)))
            {
                // Remove the error from the key's collection.
                ValidationMessages[property].Remove(
                    ValidationMessages[property].FirstOrDefault(msg => msg == message || msg.Message.Equals(message.Message)));

                OnValidationChanged(new ValidationChangedEventArgs(property, ValidationMessages[property]));
            }
        }

        /// <summary>
        /// Determines whether this instance has validation messages for the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// Returns true if validation messages exist for the given property.
        /// </returns>
        public bool HasValidationMessages(string property = "")
        {
            if (string.IsNullOrEmpty(property) || !ValidationMessages.ContainsKey(property))
            {
                return ValidationMessages.Values.Any(collection => collection.Any());
            }

            return ValidationMessages.ContainsKey(property) &&
                ValidationMessages[property].Any();
        }

        /// <summary>
        /// Determines whether this instance has validation messages for the specified property, whose IMessage implementation matches the Type given.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="property">The property.</param>
        /// <returns>
        /// Returns true if validation messages exist for the given property.
        /// </returns>
        public bool HasValidationMessages(Type messageType, string property = "")
        {
            if (string.IsNullOrEmpty(property) || !ValidationMessages.ContainsKey(property))
            {
                return ValidationMessages.Values.Any(collection => collection.Any(item => item.GetType() == messageType));
            }

            return ValidationMessages.ContainsKey(property) &&
                ValidationMessages[property].Any(collection => collection.GetType() == messageType);
        }

        /// <summary>
        /// Determines whether this instance has validation messages for the specified property, whose IMessage implementation matches the Type given.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns>
        /// Returns true if validation messages exist for the given property.
        /// </returns>
        public bool HasValidationMessages<TMessage>(string property = "") where TMessage : IValidationMessage, new()
        {
            if (string.IsNullOrEmpty(property) || !ValidationMessages.ContainsKey(property))
            {
                return ValidationMessages.Values.Any(collection => collection.Any(item => item is TMessage));
            }

            return ValidationMessages.ContainsKey(property) &&
                ValidationMessages[property].Any(message => message is TMessage);
        }

        /// <summary>
        /// Gets a key/value representation of this instances validation messages.
        /// </summary>
        /// <returns>
        /// Returns a key/value pair. The key represents the instances property names while the value holds a collection of validation messages for the property.
        /// </returns>
        public Dictionary<string, IEnumerable<IValidationMessage>> GetValidationMessages()
        {
            var messages = new Dictionary<string, IEnumerable<IValidationMessage>>();

            // We have to iterate over the collection in order to conver the messages collection
            // from a ICollection type to an IEnumerable type.
            foreach (KeyValuePair<string, ICollection<IValidationMessage>> pair in ValidationMessages)
            {
                messages.Add(pair.Key, pair.Value);
            }

            return messages;
        }

        /// <summary>
        /// Gets the validation messages for the given property..
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// Returns a collection of validation messages.
        /// </returns>
        public IEnumerable<IValidationMessage> GetValidationMessages(string property)
        {
            if (ValidationMessages.ContainsKey(property))
            {
                return ValidationMessages[property].ToArray();
            }

            // If no validation messages exist, return an empty collection.
            return new Collection<IValidationMessage>();
        }

        /// <summary>
        /// Performs validation on all of the instances properties that are eligible for validation.
        /// </summary>
        public virtual void ValidateAll()
        {
            RemoveValidationMessages();
            Dictionary<PropertyInfo, IEnumerable<IValidationRule>> cache = PropertyValidationCache[GetType()];

            foreach (KeyValuePair<PropertyInfo, IEnumerable<IValidationRule>> pair in cache)
            {
                foreach (IValidationRule rule in pair.Value)
                {
                    PerformValidation(rule, pair.Key);
                }

                // Publish our new validation collection for this property.
                OnValidationChanged(new ValidationChangedEventArgs(pair.Key.Name, ValidationMessages[pair.Key.Name]));
            }
        }

        /// <summary>
        /// Validates the specified property if the property is decorated with an IValidationRule attribute.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void ValidateProperty(string propertyName = "")
        {
            // If no property is provided, we assume we are to validate everything.
            if (string.IsNullOrEmpty(propertyName))
            {
                ValidateAll();
                return;
            }

            RemoveValidationMessages(propertyName);
            var cache = PropertyValidationCache[GetType()];
            PropertyInfo property = cache.Keys.FirstOrDefault(p => p.Name.Equals(propertyName));

            if (property == null)
            {
                throw new ArgumentException("The provided property does not contain any validation rules.", "propertyName");
            }
            foreach (IValidationRule rule in cache[property])
            {
                PerformValidation(rule, property);
            }
        }

        /// <summary>
        /// Validates the specified property via the supplied method delegate.
        /// </summary>
        /// <param name="validationDelegate">The validation delegate.</param>
        /// <param name="failureMessage">The failure message.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="validationProxy">The validation proxy.</param>
        /// <returns>Returns an IMessage if validation was not successful, otherwise null is returned to indicate success.</returns>
        public IValidationMessage ValidateProperty(Func<bool> validationDelegate, IValidationMessage failureMessage, string propertyName, IValidatable validationProxy = null)
        {
            if (validationProxy != null)
            {
                return validationProxy.ValidateProperty(validationDelegate, failureMessage, propertyName);
            }

            bool passedValidation = validationDelegate();
            if (!passedValidation)
            {
                AddValidationMessage(failureMessage, propertyName);
            }
            else
            {
                RemoveValidationMessage(failureMessage, propertyName);
            }

            return !passedValidation ? failureMessage : null;
        }

        /// <summary>
        /// Refreshes the validation.
        /// </summary>
        /// <param name="property">The property.</param>
        public void RefreshValidation(string property)
        {
            if (!string.IsNullOrEmpty(property) && HasValidationMessages(property))
            {
                ValidateProperty(property);
            }
			
            OnValidationChanged(new ValidationChangedEventArgs(property, ValidationMessages[property]));
        }

        /// <summary>
        /// Performs the given validation rule for the specified property.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <param name="property">The property.</param>
        /// <param name="validationProxy">A validation proxy can be specified.
        /// When supplied, the validation rule and any errors associated with it will be stored within the proxy object instead of this instance.</param>
        /// <exception cref="System.ArgumentNullException">
        /// PerformValidation requires a registered property to be specified.
        /// or
        /// PerformValidation requires a registered property to be specified.
        /// </exception>
        public void PerformValidation(IValidationRule rule, string property, IValidatable validationProxy = null)
        {
            PropertyInfo propertyInfo;
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentNullException("PerformValidation requires a registered property to be specified.");
            }
            else
            {
                propertyInfo = PropertyValidationCache[GetType()]
                    .FirstOrDefault(kv => kv.Key.Name.Equals(property)).Key;
            }

            if (propertyInfo == null)
            {
                throw new ArgumentNullException("PerformValidation requires a registered property to be specified.");
            }

            if (validationProxy != null && validationProxy is IValidatable)
            {
                var proxy = validationProxy;
                proxy.PerformValidation(rule, propertyInfo.Name);
            }
            else
            {
                IValidationMessage result = rule.Validate(propertyInfo, this);
                if (result != null)
                {
                    AddValidationMessage(result, propertyInfo.Name);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:ValidationChanged" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ValidationChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnValidationChanged(ValidationChangedEventArgs args)
        {
            EventHandler<ValidationChangedEventArgs> handler = ValidationChanged;

            if (handler == null)
            {
                return;
            }

            handler(this, args);
        }

        /// <summary>
        /// Registers a property so observers can accessing its ValidationMessages, even when zero exist.
        /// Any property that contains an attribute implementing IValidationRule will be automatically registered.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void RegisterProperty(params string[] propertyName)
        {
            foreach (string property in propertyName)
            {
                if (!ValidationMessages.ContainsKey(property))
                {
                    ValidationMessages[property] = new List<IValidationMessage>();
                }
            }
        }

        /// <summary>
        /// Executes the validation rule supplied for the specified property.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <param name="property">The property.</param>
        /// <param name="validationProxy">The validation proxy.</param>
        private void PerformValidation(IValidationRule rule, PropertyInfo property, IValidatable validationProxy = null)
        {
            if (validationProxy != null && validationProxy is ValidatableBase)
            {
                var proxy = validationProxy as ValidatableBase;
                proxy.PerformValidation(rule, property);
            }

            var result = rule.Validate(property, this);

            if (result != null)
            {
                AddValidationMessage(result, property.Name);
            }
        }

        /// <summary>
        /// Setups the validation, caching validation rules and properties as needed for re-use by other objects.
        /// </summary>
        private void SetupValidation()
        {
            // We instance a cache of property info's and validation rules. If any other Type is instanced that matches ours,
            // we won't need to use reflection to obtain it's members again. We will just hit the cache.
            var cache = new Dictionary<PropertyInfo, IEnumerable<IValidationRule>>();

            if (!PropertyValidationCache.ContainsKey(GetType()))
            {
                IEnumerable<PropertyInfo> propertiesToValidate = GetType().GetTypeInfo().DeclaredProperties
                    .Where(p => p.GetCustomAttributes(typeof(ValidationAttribute), true).Any());

                // Loop through all property info's and build a collection of validation rules for each property.
                foreach (PropertyInfo property in propertiesToValidate)
                {
                    var validationRules = property.GetCustomAttributes(typeof(ValidationAttribute), true) as IEnumerable<IValidationRule>;

                    if (validationRules.Any(rule => rule.ValidationMessageType == null))
                    {
                        throw new ArgumentNullException(
                            $"Validation Rule {validationRules.FirstOrDefault(rule => rule.ValidationMessageType == null).GetType().Name} does not have a" +
                            $" ValidationMessageType assigned to it for {property.Name}.");
                    }
                    cache.Add(property, validationRules);
                }

                PropertyValidationCache[GetType()] =
                    new Dictionary<PropertyInfo, IEnumerable<IValidationRule>>(cache);
            }
            else
            {
                cache = PropertyValidationCache[GetType()];
            }

            // Register each property for this instance once we are done caching.
            foreach (PropertyInfo property in cache.Keys)
            {
                RegisterProperty(property.Name);
            }
        }
    }
}
