using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Domain
{
    public abstract class Aggregate : Entity
    {
        private readonly List<IDomainEvent> _changes = new();
        private readonly List<IDomainEvent> _history = new();

        public long Version { get; private set; } = -1;

        public IReadOnlyCollection<IDomainEvent> Changes => _changes.AsReadOnly();

        public IReadOnlyCollection<IDomainEvent> History => _history.AsReadOnly();

        protected void Apply(IDomainEvent @event)
        {
            _changes.Add(@event);
            When(@event);
        }

        public void Load(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
            {
                _history.Add(@event);
                When(@event);
                Version++;
            }
        }

        protected abstract void When(IDomainEvent domainEvent);

        public void ClearChanges() => _changes.Clear();
    }
}
