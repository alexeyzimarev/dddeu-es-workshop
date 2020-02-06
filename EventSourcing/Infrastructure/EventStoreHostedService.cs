using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Microsoft.Extensions.Hosting;

namespace EventSourcing.Infrastructure
{
    public class EventStoreHostedService : IHostedService
    {
        readonly IEventStoreConnection _connection;
        readonly IProjection[] _projections;
        EventStoreSubscription _subscription;

        public EventStoreHostedService(IEventStoreConnection connection,
            params IProjection[] projections)
        {
            _connection = connection;
            _projections = projections;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _connection.ConnectAsync();
            
            _subscription = new EventStoreSubscription(_connection, _projections);
            _subscription.Start();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close();
            return Task.CompletedTask;
        }
    }
}