using EventSourcing.Domain;
using EventSourcing.Infrastructure;

namespace EventSourcing.Application
{
    public class ScreeningAppService : ApplicationService<Screening>
    {
        public ScreeningAppService(IAggregateStore aggregateStore) : base(aggregateStore)
        {
        }
    }
}