using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xerris.DotNet.Core.Core.Extensions;

namespace Xerris.DotNet.Core.Core.Validation
{
    public static class ValidationExtensions
    {
        // this regex handles base 10 numeric values but not commas.
        private static readonly Regex numericRegex = new Regex(@"^([-+]?[0-9]\d*(\.\d+)?|[0]*\.\d\d*)$");

        private static readonly Regex emailRegex =
            new Regex(
                @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"); // http://www.regexlib.com/REDetails.aspx?regexp_id=541

        // Postal codes do not use the letters D,F,I,O,Q,U. In first position, letters W,Z also not used. Space required between first and second parts.
        private static readonly Regex postalCodeRegex =
            new Regex(@"^[ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ][\s]{1}\d[ABCEGHJKLMNPRSTVWXYZ]\d$");

        // 1st digit of area code must be 2-9, 1st digit of 7 digit number must be 2-9, hyphens required, no letters
        private static readonly Regex phoneNumberRegex =
            new Regex(@"^[2-9]\d\d-[2-9]\d\d-\d{4}$");

        public static bool IsValid(this Validation validation)
        {
            return validation?.Errors == null || !validation.Errors.Any();
        }

        public static Validation IsEqual<T>(this Validation validation, T left, T right, string message)
        {
            return Equals(left, right) ? validation : validation.AddException(new ValidationException(message));
        }

        public static Validation IsFalse(this Validation validation, bool val, string message)
        {
            return val ? validation.AddException(new ValidationException(message)) : validation;
        }

        public static Validation IsTrue(this Validation validation, bool val, string message)
        {
            return val ? validation : validation.AddException(new ValidationException(message));
        }

        public static Validation IsTrue(this Validation validation, string theString,
            Func<string, bool> isValidCustomFunction, string exceptionMessage)
        {
            return !isValidCustomFunction(theString)
                ? validation.AddException(ValidationException.IsNotTrue(exceptionMessage))
                : validation;
        }

        public static Validation IsNotNull<T>(this Validation validation, T theObject, string paramName) where T : class
        {
            return theObject == null ? validation.AddException(ValidationException.IsRequired(paramName)) : validation;
        }

        public static Validation IsNotNull(this Validation validation, int theObject, string paramName)
        {
            return theObject == 0 ? validation.AddException(ValidationException.IsRequired(paramName)) : validation;
        }

        public static Validation IsNotNull(this Validation validation, decimal theObject, string paramName)
        {
            return theObject == 0 ? validation.AddException(ValidationException.IsRequired(paramName)) : validation;
        }

        public static Validation IsNotNull(this Validation validation, DateTime theObject, string paramName)
        {
            return Equals(theObject, DateTime.MinValue)
                ? validation.AddException(ValidationException.IsRequired(paramName))
                : validation;
        }

        public static Validation IsNotNull<T>(this Validation validation, T? theObject, string paramName)
            where T : struct
        {
            return theObject == null ? validation.AddException(ValidationException.IsRequired(paramName)) : validation;
        }

        public static Validation IsNull<T>(this Validation validation, T? theObject, string paramName) where T : struct
        {
            return theObject != null
                ? validation.AddException(ValidationException.FormattedError("{0} should be null", paramName))
                : validation;
        }

        public static Validation IsNull(this Validation validation, object theObject, string paramName)
        {
            return theObject != null
                ? validation.AddException(ValidationException.FormattedError("{0} should be null", paramName))
                : validation;
        }

        public static Validation IsWithinLength(this Validation validation, string theString, int maxLength,
            string paramName)
        {
            return string.IsNullOrEmpty(theString) || theString.Length > maxLength
                ? validation.AddException(ValidationException.ExceedsMaximumLength(paramName))
                : validation;
        }

        public static Validation IsLengthOf(this Validation validation, string theString, int expectedLength,
            string paramName)
        {
            var adds = expectedLength > 1 ? "s" : "";
            return string.IsNullOrEmpty(theString) || theString.Length != expectedLength
                ? validation.AddException(
                    ValidationException.IsNotLengthOf(
                        $"{paramName} needs to be exactly {expectedLength} character{adds} long."))
                : validation;
        }

        public static Validation IsNotEmpty(this Validation validation, string theString, string paramName)
        {
            return string.IsNullOrEmpty(theString)
                ? validation.AddException(ValidationException.IsRequired(paramName))
                : validation;
        }

        public static Validation IsNumeric(this Validation validation, object theValue, string paramName)
        {
            return string.IsNullOrEmpty(theValue?.ToString()) || !numericRegex.IsMatch(theValue.ToString())
                ? validation.AddException(ValidationException.MustBeNumeric(paramName))
                : validation;
        }

        public static Validation IsNotEmpty<T>(this Validation validation, IEnumerable<T> theList, string paramName)
        {
            return theList == null || !theList.Any()
                ? validation.AddException(
                    ValidationException.FormattedError("{0} is not expected to be empty", paramName))
                : validation;
        }

        public static Validation IsEmpty(this Validation validation, string theString, string paramName)
        {
            return !string.IsNullOrEmpty(theString)
                ? validation.AddException(ValidationException.FormattedError("{0} should be empty", paramName))
                : validation;
        }

        public static Validation IsEmpty<T>(this Validation validation, IEnumerable<T> theItems, string paramName)
        {
            return theItems != null && theItems.Any()
                ? validation.AddException(ValidationException.FormattedError("{0} should be empty", paramName))
                : validation;
        }

        public static Validation IsNotEqual<T>(this Validation validation, T theObject, T comparison, string paramName)
        {
            return Equals(theObject, comparison)
                ? validation.AddException(ValidationException.IsRequired(paramName))
                : validation;
        }

        public static Validation GreaterThan(this Validation validation, DateTime? theObject, DateTime? comparison,
            string paramName)
        {
            if (!theObject.HasValue || !comparison.HasValue) return validation;
            return theObject.Value.CompareTo(comparison.Value) > 0
                ? validation
                : validation.AddException(new ValidationException(paramName));
        }

        public static Validation GreaterThan<T>(this Validation validation, T theObject, T comparison, string paramName)
            where T : IComparable<T>
        {
            return theObject.CompareTo(comparison) > 0
                ? validation
                : validation.AddException(new ValidationException(paramName));
        }

        public static Validation GreaterThanOrEqual(this Validation validation, DateTime? theObject,
            DateTime? comparison,
            string paramName)
        {
            if (!theObject.HasValue || !comparison.HasValue) return validation;
            return GreaterThanOrEqual(validation, theObject.Value, comparison.Value, paramName);
        }

        public static Validation GreaterThanOrEqual<T>(this Validation validation, T theObject, T comparison,
            string paramName) where T : IComparable
        {
            return theObject.CompareTo(comparison) >= 0
                ? validation
                : validation.AddException(new ValidationException(paramName));
        }

        public static Validation Check(this Validation validation)
        {
            if (validation == null) return null;
            if (!validation.Errors.Any()) return validation;
            if (validation.Errors.Count() == 1)
                throw new ValidationException("Validation Failure", validation.Errors.First());
            throw new ValidationException("Validation Failure", new MultiException(validation.Errors));
        }

        public static ValidationException Warnings(this Validation validation)
        {
            if (validation == null) return null;
            var enumerable = validation.Warnings.ToArray();
            return enumerable.Length == 1 ? enumerable.First() : new MultiException(enumerable);
        }

        public static Validation IsOfType<T>(this Validation validation, object theObject, string paramName)
        {
            if (theObject == null)
                return validation.AddException(ValidationException.IsRequired(paramName));
            if (theObject.GetType() != typeof(T))
                return
                    validation.AddException(
                        new ValidationException($"{paramName} is not of type {typeof(T).Name}"));
            return validation;
        }

        public static Validation Contains<T>(this Validation validation, IEnumerable<T> items, T criteria,
            string message)
        {
            return items.Any(x => Equals(x, criteria))
                ? validation
                : validation.AddException(new ValidationException(message));
        }

        public static Validation Contains<T>(this Validation validation, IEnumerable<T> items, Predicate<T> criteria,
            string message)
        {
            return items.Any(x => criteria(x))
                ? validation
                : validation.AddException(new ValidationException(message));
        }

        public static Validation Or(this Validation validation, string message, params bool[] criteria)
        {
            return criteria.Any() ? validation.Add(new ValidationException(message)) : validation;
        }

        public static Validation AddException(this Validation validation, ValidationException exception)
        {
            return (validation ?? new Validation()).Add(exception);
        }

        public static Validation IsEmail(this Validation validation, string theValue, string paramName)
        {
            return string.IsNullOrEmpty(theValue) || !emailRegex.IsMatch(theValue)
                ? validation.AddException(ValidationException.MustBeEmail(paramName))
                : validation;
        }

        public static Validation IsPostalCode(this Validation validation, string theValue, string paramName)
        {
            return string.IsNullOrEmpty(theValue) || !postalCodeRegex.IsMatch(theValue)
                ? validation.AddException(ValidationException.MustBePostalCode(paramName))
                : validation;
        }

        public static Validation IsPhoneNumber(this Validation validation, string theValue, string paramName)
        {
            return string.IsNullOrEmpty(theValue) || !phoneNumberRegex.IsMatch(theValue)
                ? validation.AddException(ValidationException.MustBePhoneNumber(paramName))
                : validation;
        }

        public static Validation Append(this Validation destination, Validation target)
        {
            if (destination == null && target == null) return null;
            if (destination == null) return target;
            if (target == null) return destination;

            target.Exceptions.ForEach(e => destination.Add(e));
            return destination;
        }

        public static Validation ContinueIfValid(this Validation validation, Func<Validation, Validation> continueWith)
        {
            if (validation == null) return continueWith(Validate.Begin());
            return validation;
        }
    }
}