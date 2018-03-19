using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace datathegenius.DatasEssentials
{
    /// <summary>
    ///  A container object which may or may not contain a non-null value.
    ///  If a value is present, <see cref="IsPresent"/> will return <code>true</code> and
    /// <see cref="Value"/> will return the value.
    /// </summary>
    public class Optional<T>
    {

        private static readonly Optional<T> EMPTY = new Optional<T>(default(T));
        private readonly T _value;

        public bool IsPresent => _value != null;

        public bool IsAbsent => _value == null;

        public T Value
        {
            get
            {
                if (_value == null)
                {
                    throw new InvalidOperationException("No value present.");
                }

                return _value;
            }
        }

        private Optional(T value)
        {
            _value = value;
        }

        public void IfPresent([NotNull] Action<T> consumer)
        {
            if (IsPresent)
            {
                consumer(Value);
            }
        }

        public void IfAbsent([NotNull] Action<T> consumer)
        {
            if (IsAbsent)
            {
                consumer(Value);
            }
        }

        public T OrElse(T value)
        {
            return IsPresent ? Value : value;
        }

        public static Optional<T> Of(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "value cannot be null");
            }

            return new Optional<T>(value);
        }

        public static Optional<T> OfNullable(T value)
        {
            return new Optional<T>(value);
        }

        public static Optional<T> Empty()
        {
            return EMPTY;
        }

        public override string ToString()
        {
            return IsPresent ? $"Optional[{Value}]" : "Optional.Empty";
        }

    }
}
