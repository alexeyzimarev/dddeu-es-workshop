using System;
using System.Threading.Tasks;

namespace EventSourcing.Application
{
    public delegate Task<int> GetMovieDuration(string movieId);

    public delegate DateTimeOffset GetUtcNow();
}