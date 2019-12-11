using System;
using System.Runtime.Serialization;

namespace Xerris.DotNet.Core.Core.Validations
{
    [Serializable]
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(Exception message) : base(message.Message)
        {
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ValidationSeverity Severity { get; set; }

        public bool IsError => Equals(ValidationSeverity.Error, Severity);

        public bool IsWarning => Equals(ValidationSeverity.Warning, Severity);

        public override string Message => InnerException != null ? InnerException.Message : base.Message;

        public ValidationException Warning()
        {
            Severity = ValidationSeverity.Warning;
            return this;
        }

        public static ValidationException IsRequired(string message)
        {
            return new ValidationException($"{message}");
        }

        public static ValidationException ExceedsMaximumLength(string message)
        {
            return new ValidationException($"{message}");
        }

        public static ValidationException IsNotLengthOf(string message)
        {
            return new ValidationException($"{message}");
        }

        public static ValidationException IsNotTrue(string message)
        {
            return new ValidationException($"{message}");
        }

        public static ValidationException MustBeNumeric(string message)
        {
            return new ValidationException($"{message}");
        }

        public static ValidationException MustBeEmail(string message)
        {
            return new ValidationException($"{message}");
        }

        public static ValidationException MustBePostalCode(string message)
        {
            return new ValidationException($"{message}");
        }

        public static ValidationException MustBePhoneNumber(string message)
        {
            return new ValidationException($"{message}");
        }

        public static ValidationException MustBeOfLength(string message)
        {
            return new ValidationException($"{message}");
        }

        public static ValidationException MustBeMinimumLength(string message)
        {
            return new ValidationException($"{message}");
        }
        
        public static ValidationException FormattedError(string format, params string[] args)
        {
            return new ValidationException(string.Format(format, args));
        }
    }
}