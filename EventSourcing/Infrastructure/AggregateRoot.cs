using System;
using System.Collections.Generic;

namespace EventSourcing.Infrastructure
{
    public abstract class AggregateRoot
    {
        protected void Apply(object evt)
        {
            _changes.Add(evt);
            When(evt);

            if (!IsEverythingStillOk())
                throw new InvalidOperationException("something went wrong");
        }

        protected abstract bool IsEverythingStillOk();

        public abstract string GetId();

        protected abstract void When(object evt);

        public void Load(object[] events)
        {
            foreach (var @event in events)
            {
                When(@event);
                Version++;
            }
        }

        List<object> _changes { get; } = new List<object>();

        public IReadOnlyCollection<object> Changes => _changes.AsReadOnly();

        public void ClearChanges() => _changes.Clear();

        public int Version { get; set; } = -1;
    }
}