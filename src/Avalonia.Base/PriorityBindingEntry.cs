// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Data;

namespace Avalonia
{
    /// <summary>
    /// A registered binding in a <see cref="PriorityValue"/>.
    /// </summary>
    internal class PriorityBindingEntry : IDisposable
    {
        private PriorityLevel _owner;
        private IDisposable _subscription;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityBindingEntry"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="index">
        /// The binding index. Later bindings should have higher indexes.
        /// </param>
        public PriorityBindingEntry(PriorityLevel owner, int index)
        {
            _owner = owner;
            Index = index;
        }

        /// <summary>
        /// Gets the observable associated with the entry.
        /// </summary>
        public IObservable<object> Observable { get; private set; }

        /// <summary>
        /// Gets a description of the binding.
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the binding entry index. Later bindings will have higher indexes.
        /// </summary>
        public int Index
        {
            get;
        }

        /// <summary>
        /// The current value of the binding.
        /// </summary>
        public object Value
        {
            get;
            private set;
        }

        /// <summary>
        /// Starts listening to the binding.
        /// </summary>
        /// <param name="binding">The binding.</param>
        public void Start(IObservable<object> binding)
        {
            Contract.Requires<ArgumentNullException>(binding != null);

            Initialize(binding);
            _subscription = binding.Subscribe(ValueChanged, Completed);
        }

        /// <summary>
        /// Starts listening to the binding.
        /// </summary>
        /// <param name="binding">The binding.</param>
        public void Start(IObservable<BindingNotification> binding)
        {
            Contract.Requires<ArgumentNullException>(binding != null);

            Initialize(binding);
            _subscription = binding.Subscribe(ReceivedBindingNotification, Completed);
        }


        /// <summary>
        /// Ends the binding subscription.
        /// </summary>
        public void Dispose()
        {
            _subscription?.Dispose();
        }

        private void Initialize(IObservable<object> binding)
        {
            Contract.Requires<ArgumentNullException>(binding != null);

            if (_subscription != null)
            {
                throw new Exception("PriorityValue.Entry.Start() called more than once.");
            }

            Observable = binding;
            Value = AvaloniaProperty.UnsetValue;

            if (binding is IDescription)
            {
                Description = ((IDescription)binding).Description;
            }
        }

        private void ValueChanged(object value)
        {
            Value = value;
            _owner.Changed(this);
        }

        private void ReceivedBindingNotification(BindingNotification notification)
        {
            if (notification.HasValue)
            {
                ValueChanged(notification.Value);
            }

            if (notification.ErrorType == BindingErrorType.Error)
            {
                _owner.Error(this, notification.Error);
            }
        }

        private void Completed()
        {
            _owner.Completed(this);
        }
    }
}
