﻿using System;
using Newtonsoft.Json;

namespace Artemis.Core
{
    /// <summary>
    ///     Represents an output pin containing a value of type <typeparamref name="T" /> on a <see cref="INode" />
    /// </summary>
    public sealed class OutputPin<T> : Pin
    {
        #region Constructors

        [JsonConstructor]
        internal OutputPin(INode node, string name)
            : base(node, name)
        {
            _value = default;
        }

        #endregion

        #region Properties & Fields

        /// <inheritdoc />
        public override Type Type { get; } = typeof(T);

        /// <inheritdoc />
        public override object? PinValue => Value;

        /// <inheritdoc />
        public override PinDirection Direction => PinDirection.Output;

        private T? _value;

        /// <summary>
        ///     Gets or sets the value of the output pin
        /// </summary>
        public T? Value
        {
            get
            {
                if (!IsEvaluated)
                    Node?.Evaluate();

                return _value;
            }
            set
            {
                _value = value;
                IsEvaluated = true;

                OnPropertyChanged(nameof(PinValue));
            }
        }

        #endregion
    }

    /// <summary>
    ///     Represents an output pin on a <see cref="INode" />
    /// </summary>
    public sealed class OutputPin : Pin
    {
        #region Constructors

        internal OutputPin(INode node, Type type, string name)
            : base(node, name)
        {
            Type = type;
            _value = type.GetDefault();
        }

        #endregion

        #region Properties & Fields

        /// <inheritdoc />
        public override Type Type { get; }

        /// <inheritdoc />
        public override object? PinValue => Value;

        /// <inheritdoc />
        public override PinDirection Direction => PinDirection.Output;

        private object? _value;

        /// <summary>
        ///     Gets or sets the value of the output pin
        /// </summary>
        public object? Value
        {
            get
            {
                if (!IsEvaluated)
                    Node?.Evaluate();

                return _value;
            }
            set
            {
                if (Type.IsValueType && value == null)
                {
                    // We can't take null for value types so set it to the default value for that type
                    _value = Type.GetDefault();
                }
                else if (value != null)
                {
                    // If a value was given make sure it matches
                    if (!Type.IsInstanceOfType(value))
                        throw new ArgumentException($"Value of type '{value.GetType().Name}' can't be assigned to a pin of type {Type.Name}.");
                }

                // Otherwise we're good and we can put a null here if it happens to be that
                _value = value;
                IsEvaluated = true;
                OnPropertyChanged(nameof(PinValue));
            }
        }

        #endregion
    }
}