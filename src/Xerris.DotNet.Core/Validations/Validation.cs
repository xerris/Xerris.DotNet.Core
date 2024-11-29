using System.Collections.Generic;
using System.Text;

namespace Xerris.DotNet.Core.Validations
{
    public sealed class Validation
    {
        private readonly List<ValidationException> exceptions;

        public Validation()
            => exceptions = new List<ValidationException>(1); // optimize for only having 1 exceptio
        

        public IEnumerable<ValidationException> Exceptions => exceptions;

        public IEnumerable<ValidationException> Errors 
            => exceptions.FindAll(e => e.IsError);


        public IEnumerable<ValidationException> Warnings
            => exceptions.FindAll(e => e.IsWarning);
        

        public Validation Add(ValidationException ex)
        {
            lock (exceptions)
            {
                exceptions.Add(ex);
            }

            return this;
        }

        public string PrettyPrint()
        {
            var builder = new StringBuilder();
            exceptions.ForEach(each => builder.AppendLine(each.Message));
            return builder.ToString().TrimEnd('\n', '\r');
        }
    }
}