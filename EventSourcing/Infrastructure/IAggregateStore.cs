using System.Threading.Tasks;

namespace EventSourcing.Infrastructure
{
    public interface IAggregateStore
    {
        Task<bool> Exists<T>(string aggregateId);
        
        Task Save<T>(T aggregate) where T : AggregateRoot;
        
        Task<T> Load<T>(string aggregateId) where T : AggregateRoot;
    }
}