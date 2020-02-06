using System.Collections.Generic;
using System.Threading.Tasks;
using EventSourcing.Infrastructure;
using MongoDB.Driver;
using static EventSourcing.Domain.ScreeningEvents.V1;

namespace EventSourcing.Projections
{
    public class ScheduledScreeningProjection : IProjection
    {
        readonly IMongoCollection<ScheduledScreenings> _collection;

        public ScheduledScreeningProjection(IMongoDatabase database)
        {
            _collection = database.GetCollection<ScheduledScreenings>("screenings");
        }

        public Task Project(object evt)
            => evt switch
            {
                ScreeningScheduled e =>
                _collection.InsertOneAsync(new ScheduledScreenings
                {
                    TheaterId = e.TheaterId,
                    Screenings = new List<ScheduledScreenings.Screening>
                    {
                        new ScheduledScreenings.Screening
                        {
                            MovieId           = e.MovieId,
                            ScheduledAt       = e.StartsAt,
                            DurationInMinutes = e.DurationInMinutes
                        }
                    }
                }),
                _ => Task.CompletedTask
            };
    }
}