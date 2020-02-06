using System;
using System.Threading.Tasks;
using EventSourcing.Domain;
using EventSourcing.Infrastructure;
using static EventSourcing.Application.ScreeningCommands.V1;

namespace EventSourcing.Application
{
    public class ScreeningAppService : ApplicationService<Screening>
    {
        readonly GetUtcNow _getUtcNow;
        readonly GetMovieDuration _getMovieDuration;

        public ScreeningAppService(IAggregateStore aggregateStore,
            GetUtcNow getUtcNow,
            GetMovieDuration getMovieDuration) : base(aggregateStore)
        {
            _getUtcNow = getUtcNow;
            _getMovieDuration = getMovieDuration;
        }

        public async Task Handle(ScheduleScreening command)
        {
            var duration = await _getMovieDuration(command.MovieId);
            
            await Handle(
                command.ScreeningId,
                screening => screening.Schedule(
                    command.ScreeningId,
                    new Movie(command.MovieId, duration),
                    new Theater(command.TheaterId),
                    _getUtcNow()
                )
            );
        }

        public Task Handle(ReserveSeat command, string userId)
            => Handle(
                command.ScreeningId,
                screening => screening.ReserveSeat(command.Row, command.Seat, userId, _getUtcNow())
            );
    }
}