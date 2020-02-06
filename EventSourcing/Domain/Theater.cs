using System.Threading.Tasks;

namespace EventSourcing.Domain
{
    public delegate Task<int> GetTheaterCapacity(string id);

    public class Theater
    {
        public Theater(string id)
        {
            Id = id;
        }

        public static async Task<Theater> FromId(string id, GetTheaterCapacity getTheaterCapacity)
            => new Theater(id) {Capacity = await getTheaterCapacity(id)};

        public string Id       { get; }
        public int    Capacity { get; private set; }
    }
}