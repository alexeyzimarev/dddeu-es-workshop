using System;
using System.Collections.Generic;
using EventSourcing.Infrastructure;
using static EventSourcing.Domain.ScreeningEvents;

namespace EventSourcing.Domain
{
    public class Screening : AggregateRoot
    {
        ScreeningState State { get; set; } = new ScreeningState();

        public void Schedule(string screeningId, Movie movie, Theater theater, DateTimeOffset startsAt)
        {
            if (Version >= 0)
                throw new InvalidOperationException("Can't do that, go away");
            
            var evt = new V1.ScreeningScheduled
            {
                ScreeningId       = screeningId,
                MovieId           = movie.Id,
                TheaterId         = theater.Id,
                DurationInMinutes = movie.DurationInMinutes,
                StartsAt          = startsAt
            };
            Apply(evt);
        }

        protected override bool IsEverythingStillOk()
        {
            return !string.IsNullOrWhiteSpace(State.Id);
        }

        public override string GetId() => State.Id;

        protected override void When(object evt)
        {
            State = evt switch
            {
                V1.ScreeningScheduled e => State.With(x => x.Id = e.ScreeningId),
                _                       => State
            };
        }

        class ScreeningState
        {
            public string Id { get; set; }
        }

        public void ReserveSeat(in int row, in int seat)
        {
            Apply(new V1.SeatReserved
            {
                ScreeningId = State.Id,
                Seat = seat,
                Row = row,
            });
        }
    }
}