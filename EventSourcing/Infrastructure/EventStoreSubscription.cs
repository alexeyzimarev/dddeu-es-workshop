using System;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace EventSourcing.Infrastructure
{
    public class EventStoreSubscription
    {
        readonly IEventStoreConnection _connection;
        readonly IProjection[] _projections;

        public EventStoreSubscription(IEventStoreConnection connection,
            params IProjection[] projections)
        {
            _connection = connection;
            _projections = projections;
        }

        public void Start()
        {
            _connection
                .SubscribeToAllFrom(
                    Position.Start,
                    CatchUpSubscriptionSettings.Default,
                    Project
                );
        }

        Task Project(EventStoreCatchUpSubscription subscription, ResolvedEvent re)
        {
            if (re.Event.EventType.StartsWith("$"))
                return Task.CompletedTask;
            
            var data = re.Deserialize();
            return Task.WhenAll(_projections.Select(x => x.Project(data)));
        }
    }
}