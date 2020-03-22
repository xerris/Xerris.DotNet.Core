using System;
using System.Collections.Generic;

namespace Xerris.DotNet.Core.Utilities
{
    [Serializable]
    public class Range<T> : IEquatable<Range<T>> where T : IComparable<T>
    {
        private readonly Func<T, T> incrementor;
        private Tuple<T, T> tuple;

        public Range(T start, T end) 
        {
            tuple = new Tuple<T, T>(start, end);
        }

        public virtual T Start
        {
            get => tuple.Item1;
            set => tuple = new Tuple<T, T>(value, tuple.Item2);
        }

        public virtual T End
        {
            get => tuple.Item2;
            set => tuple = new Tuple<T, T>(tuple.Item1, value);
        }

        public Range(T start, T end, Func<T, T> inc) : this(start, end)
        {
            incrementor = inc;
        }
        
        public virtual bool Includes(T value)
        {
            return Start.CompareTo(value) <= 0 && End.CompareTo(value) >= 0;
        }

        public virtual bool Includes(Range<T> value) //ToDo: Unit tests? [MR]
        {
            return Start.CompareTo(value.Start) <= 0 && End.CompareTo(value.End) >= 0;
        }

        public virtual bool Overlaps(Range<T> value) //ToDo: Unit tests? [MR]
        {
            return Start.CompareTo(value.End) <= 0 && End.CompareTo(value.Start) >= 0;
        }

        public virtual void ForEach(Action<T> action)
        {
            ForEach(incrementor, action);
        }

        public virtual void ForEach(Func<T,T> incAction, Action<T> action)
        {
            if(incAction == null)
            {
                throw new NullReferenceException("An incrementor action is required.");
            }
            for (var current = Start; Includes(current); current=incAction(current))
            {
                action(current);
            }
        }

        public virtual void BackwardForEach(Func<T, T> decrementorAction, Action<T> action)
        {
            if (decrementorAction == null)
            {
                throw new NullReferenceException("An decrementor action is required.");
            }
            for (var current = End; Includes(current); current = decrementorAction(current))
            {
                action(current);
            }
        }
        
        public static Range<int> Create(int start, int end)
        {
            return new Range<int>(start, end, inc=>inc+1);
        }

        public virtual bool AssertIncludes(T value)
        {
            if (!Includes(value))
                throw new ArgumentException($"value is outside range {value}");
            return true;
        }

        public virtual T[] ToArray()
        {
            var list = new List<T>();
            ForEach(list.Add);
            return list.ToArray();
        }

        [IgnoreForReflectionEquals("Imagine iterating over every day from 2014-08-01 to 9999-08-01 with Reflection Equals. That will take a while just to do a ReflectionEquals.")]
        public IEnumerable<T> Iterate
        {
            get
            {
                yield return Start;
                var item = incrementor(Start);
                while (End.CompareTo(item) > 0)
                {
                    yield return item;
                    item = incrementor(item);
                }
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Range<T> range && Equals(range);
        }

        public bool Equals(Range<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(incrementor, other.incrementor) && Equals(tuple, other.tuple);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((incrementor != null ? incrementor.GetHashCode() : 0)*397) ^ (tuple != null ? tuple.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return $"Left={tuple.Item1}, Right={tuple.Item2}";
        }
    }
}