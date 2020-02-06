using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace EventSourcing.Infrastructure
{
    public class EsAggregateStore : IAggregateStore
    {
        readonly IEventStoreConnection _connection;

        public EsAggregateStore(IEventStoreConnection connection)
        {
            _connection = connection;
        }

        public async Task Save<T>(T aggregate) where T : AggregateRoot
        {
            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            var streamName = GetStreamName(aggregate);
            var changes = aggregate.Changes.ToArray();

            await _connection.AppendEvents(streamName, aggregate.Version, changes);

            aggregate.ClearChanges();
        }

        public async Task<T> Load<T>(string aggregateId) where T : AggregateRoot
        {
            if (aggregateId == null)
                throw new ArgumentNullException(nameof(aggregateId));

            var stream = GetStreamName<T>(aggregateId);
            var aggregate = (T) Activator.CreateInstance(typeof(T), true);

            var page = await _connection.ReadStreamEventsForwardAsync(
                stream, 0, 1024, false);

            aggregate.Load(page.Events.Select(
                resolvedEvent => resolvedEvent.Deserialize()).ToArray());

            return aggregate;
        }

        public async Task<bool> Exists<T>(string aggregateId)
        {
            var stream = GetStreamName<T>(aggregateId);
            var result = await _connection.ReadEventAsync(stream, 1, false);
            return result.Status != EventReadStatus.NoStream;
        }

        static byte[] Serialize(object data)
            => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

        static string GetStreamName<T>(string aggregateId)
            => $"{typeof(T).Name}-{aggregateId}";

        static string GetStreamName<T>(T aggregate)
            where T : AggregateRoot
            => $"{typeof(T).Name}-{aggregate.GetId()}";
    }
}