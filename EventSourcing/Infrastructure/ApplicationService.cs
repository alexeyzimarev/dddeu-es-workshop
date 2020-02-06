using System;
using System.Threading.Tasks;

namespace EventSourcing.Infrastructure
{
    public abstract class ApplicationService<TAggregate>
        where TAggregate : AggregateRoot
    {
        readonly IAggregateStore _aggregateStore;

        protected ApplicationService(IAggregateStore aggregateStore)
        {
            _aggregateStore = aggregateStore;
        }

        public async Task Handle(string id, Action<TAggregate> whatToDo)
        {
            var aggregate = await _aggregateStore.Load<TAggregate>(id);

            whatToDo(aggregate);

            await _aggregateStore.Save(aggregate);
        }
    }
}