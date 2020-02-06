using System.Threading.Tasks;

namespace EventSourcing.Infrastructure
{
    public interface IProjection
    {
        Task Project(object evt);
    }
}