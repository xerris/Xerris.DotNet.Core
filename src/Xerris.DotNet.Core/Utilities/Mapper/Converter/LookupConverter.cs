using System.Collections.Generic;

namespace Xerris.DotNet.Core.Utilities.Mapper.Converter
{
    public class LookupConverter<TFrom,TO> : IValueConverter<TO>
    {
        private readonly TO defaultValue;
        private readonly Dictionary<TFrom, TO> conversions;

        public LookupConverter(TO defaultValue, Dictionary<TFrom, TO> conversions)
        {
            this.defaultValue = defaultValue;
            this.conversions = conversions;
        }

        public TO Convert(object input) => !(input is TFrom) ? defaultValue : Get((TFrom) input);

        private TO Get(TFrom input) => 
            Equals(default(TFrom), input) || !conversions.ContainsKey(input) ? defaultValue : conversions[input];
    }
}
