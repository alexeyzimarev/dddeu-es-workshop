using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Microsoft.Extensions.Hosting;

namespace EventSourcing.Infrastructure
{
    public class EventStoreHostedService : IHostedService
    {
        readonly IEventStoreConnection _connection;

        public EventStoreHostedService(IEventStoreConnection connection) => _connection = connection;

        public Task StartAsync(CancellationToken cancellationToken) => _connection.ConnectAsync();

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close();
            return Task.CompletedTask;
        }
    }
}