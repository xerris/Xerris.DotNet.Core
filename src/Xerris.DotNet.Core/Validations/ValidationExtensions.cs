using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Xerris.DotNet.Core.Extensions;

namespace Xerris.DotNet.Core.Validations
{
    public static class ValidationExtensions
    {
        // this regex handles base 10 numeric values but not commas.
        private static readonly Regex NumericRegex = new Regex(@"^([-+]?[0-9]\d*(\.\d+)?|[0]*\.\d\d*)$");

        private static readonly Regex EmailRegex =
            new Regex(
                @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"); // http://www.regexlib.com/REDetails.aspx?regexp_id=541

        // Postal codes do not use the letters D,F,I,O,Q,U. In first position, letters W,Z also not used. Space required between first and second parts.
        private static readonly Regex PostalCodeRegex =
            new Regex(@"^[ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ][\s]{1}\d[ABCEGHJKLMNPRSTVWXYZ]\d$");

        // 1st digit of area code must be 2-9, 1st digit of 7 digit number must be 2-9, hyphens not required, no letters
        private static readonly Regex PhoneNumberRegex =
            new Regex(@"^(?:\(?)[2-9](\d{2})[- ]?(?:[\).\s]?)[2-9](\d{2})[- ]?(?:[-\.\s]?)(\d{4})(?!\d)$");

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
            return val == false? validation : validation.AddException(new ValidationException(message));
        }

        public static Validation IsTrue(this Validation validation, bool val, string message)
        {
            return val ? validation : validation.AddException(new ValidationException(message));
        }

        public static Validation IsTrue(this Validation validation, bool? val, string message)
        {
            return val.HasValue && val.Value? validation : validation.AddException(new ValidationException(message));
        }

        public static Validation IsFalse(this Validation validation, bool? val, string message)
        {
            return val.HasValue && val.Value==false? validation : validation.AddException(new ValidationException(message));
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
            return string.IsNullOrEmpty(theValue?.ToString()) || !NumericRegex.IsMatch(theValue.ToString())
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

        public static Validation LessThanOrEqual<T>(this Validation validation, T theObject, T comparison,
            string paramName) where T : IComparable
        {
            return theObject.CompareTo(comparison) <= 0
                ? validation
                : validation.AddException(new ValidationException(paramName));
        }

        public static Validation LessThanOrEqual(this Validation validation, DateTime? theObject,
            DateTime? comparison,
            string paramName)
        {
            if (!theObject.HasValue || !comparison.HasValue) return validation;
            return LessThanOrEqual(validation, theObject.Value, comparison.Value, paramName);
        }

        public static Validation LessThan<T>(this Validation validation, T theObject, T comparison, string paramName)
            where T : IComparable<T>
        {
            return theObject.CompareTo(comparison) < 0
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

        [Obsolete("use HasExactly instead")]
        public static Validation HasItems<T>(this Validation validation, IEnumerable<T> items, int count, string message)
        {
            return (items != null && items.Count() == count)
                    ? validation
                    : validation.AddException(new ValidationException(message));
        }
        
        public static Validation HasExactly<T>(this Validation validation, IEnumerable<T> items, int expected, string message)
        {
            return (items != null && items.Count() == expected)
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
            return string.IsNullOrEmpty(theValue) || !EmailRegex.IsMatch(theValue)
                ? validation.AddException(ValidationException.MustBeEmail(paramName))
                : validation;
        }

        public static Validation IsPostalCode(this Validation validation, string theValue, string paramName)
        {
            return string.IsNullOrEmpty(theValue) || !PostalCodeRegex.IsMatch(theValue)
                ? validation.AddException(ValidationException.MustBePostalCode(paramName))
                : validation;
        }

        public static Validation IsPhoneNumber(this Validation validation, string theValue, string paramName)
        {
            return string.IsNullOrEmpty(theValue) || !PhoneNumberRegex.IsMatch(theValue)
                ? validation.AddException(ValidationException.MustBePhoneNumber(paramName))
                : validation;
        }
        
        public static Validation IsDate(this Validation validation, string date, string paramName, string format = null)
        {
            var success = format == null
                ? DateTime.TryParse(date, out _)
                : DateTime.TryParseExact(date, format, DateTimeFormatInfo.CurrentInfo,
                    DateTimeStyles.AssumeLocal, out _);

            if (!success)
            {
                validation.AddException(new ValidationException($"{paramName} is not a valid Date"));
            }

            return validation;
        }
        
        public static Validation IsMinimumLength(this Validation validation, string theValue, int minimumLength,
            string message)
        {
            return string.IsNullOrEmpty(theValue) || theValue.Length < minimumLength
                ? validation.AddException(ValidationException.MustBePhoneNumber(message))
                : validation;
        }

        public static Validation HasLength(this Validation validation, string theValue, int length, string message)
        {
            return (theValue ?? "").Length != length
                ? validation.AddException(ValidationException.MustBeOfLength(message))
                : validation;
        }
        
        public static Validation Append(this Validation destination, Validation target)
        {
            switch (destination)
            {
                case null when target == null:
                    return null;
                case null:
                    return target;
            }

            if (target == null) return destination;

            target.Exceptions.ForEach(e => destination.Add(e));
            return destination;
        }

        public static Validation ContinueIfValid(this Validation validation, Func<Validation, Validation> continueWith)
        {
            return validation ?? continueWith(Validate.Begin());
        }

        public static Validation ForEach<T>(this Validation validation, IEnumerable<T> items, Func<Validation, T, Validation> action)
        {
            return items == null
                ? validation.AddException(ValidationException.ListIsNullOrEmpty("items"))
                : items.Aggregate(validation, action);
        }
        
        public static Validation TypeIsEqual<T>(this Validation validation, object theObject, string paramName)
        {
            return validation
                .IsNotNull(theObject, "theObject").Check()
                .IsEqual(typeof (T), theObject.GetType(), paramName);
        }

        public static Validation Is<T>(this Validation validation, T value, Func<Validation, T, Validation> validateAction)
        {
            return validateAction(validation, value);
        }
        
        public static Validation ComparesTo<T>(this Validation validation, T x, T y, Action<Validation, T, T> validateAction) 
            where T : class
        {
            validation.IsNotNull(x, "x").Check()
                .IsNotNull(y, "y").Check();
            validateAction(validation, x, y);
            return validation;
        } 
        
        public static Validation ComparesTo<T,U>(this Validation validation, T t, U u, Action<Validation, T, U> validateAction) 
            where T : class where U : class
        {
            validation.IsNotNull(t, "t is null").Check()
                .IsNotNull(u, "u is null").Check();
            validateAction(validation, t, u);
            return validation;
        } 
        
        public static Validation IsInteger(this Validation validate, string value, string property)
        {
            var regex = new Regex("^([+-]?[1-9]\\d*|0)$");
            return validate
                .IsNotEmpty(value, $"{property} is empty")
                .ContinueIfValid(v => v.IsTrue(regex.IsMatch(value), $"{property} is not an integer"));
        }
        
        public static Validation IsPositiveInteger(this Validation validate, string value, string property)
        {
            return validate
                .IsInteger(value, property)
                .ContinueIfValid(v1 => v1.GreaterThan(int.Parse(value), 0, $"{property} is notgreater than 0"));
        }

        public static Validation IsPrice(this Validation validate, string value, string property)
        {
            return validate
                .IsDecimal(value, property)
                .ContinueIfValid(v1 => v1.GreaterThan(decimal.Parse(value), 0, $"{property} is not greater than 0"));

        }

        public static Validation IsDecimal(this Validation validate, string value, string property)
        {            
            var regex = new Regex("^[+-]?\\d*(\\.\\d*)?$");
            return validate
                .IsNotEmpty(value, $"{property} is empty")
                .ContinueIfValid(v1 => v1.IsTrue(regex.IsMatch(value), $"{property} is not a decimal"));
        }

        public static Validation Throw(this Validation validation, string message)
        {
            return validation.AddException(new ValidationException(message)).Check();
        }
    }
}