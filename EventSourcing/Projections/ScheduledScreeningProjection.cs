using System.Collections.Generic;
using System.Threading.Tasks;
using EventSourcing.Infrastructure;
using MongoDB.Driver;
using static EventSourcing.Domain.ScreeningEvents.V1;
using static MongoDB.Driver.Builders<EventSourcing.Projections.ScheduledScreenings>;

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
                _collection.UpdateOneAsync(
                    Filter.Eq(x => x.TheaterId, e.TheaterId),
                    Update
                        .Push(x => x.Screenings,
                        new ScheduledScreenings.Screening
                        {
                            ScreeningId       = e.ScreeningId,
                            MovieId           = e.MovieId,
                            ScheduledAt       = e.StartsAt,
                            DurationInMinutes = e.DurationInMinutes,
                            SeatsLeft         = e.TheaterCapacity
                        }
                    ),
                    new UpdateOptions {IsUpsert = true}
                ),
                SeatReserved e =>
                _collection.UpdateOneAsync(
                    Filter.ElemMatch(
                        x => x.Screenings,
                        screening => screening.ScreeningId == e.ScreeningId),
                    Update.Set(x => x.Screenings[-1].SeatsLeft, e.SeatsLeft)
                ),
                _ => Task.CompletedTask
            };
    }
}