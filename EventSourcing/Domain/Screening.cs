using System;
using System.Collections.Generic;
using System.Linq;
using EventSourcing.Infrastructure;
using static EventSourcing.Domain.ScreeningEvents;

namespace EventSourcing.Domain
{
    public class Screening : AggregateRoot
    {
        ScreeningState State { get; set; } = new ScreeningState();

        public void Schedule(
            string screeningId,
            Movie movie, Theater theater,
            DateTimeOffset startsAt
        )
        {
            if (Version >= 0)
                throw new InvalidOperationException("Can't do that, go away");

            var evt = new V1.ScreeningScheduled
            {
                ScreeningId       = screeningId,
                MovieId           = movie.Id,
                TheaterId         = theater.Id,
                TheaterCapacity   = theater.Capacity,
                DurationInMinutes = movie.DurationInMinutes,
                StartsAt          = startsAt
            };
            Apply(evt);
        }

        protected override bool IsEverythingStillOk()
        {
            return !string.IsNullOrWhiteSpace(State.Id)
                && State.Seats.Distinct().Count() == State.Seats.Count
                && State.Capacity                 >= 0;
        }

        public override string GetId() => State.Id;

        protected override void When(object evt)
        {
            State = evt switch
            {
                V1.ScreeningScheduled e => State.With(x =>
                {
                    x.Id       = e.ScreeningId;
                    x.Capacity = e.TheaterCapacity;
                }),
                V1.SeatReserved e => State.When(e),
                _                 => State
            };
        }

        class ScreeningState
        {
            public string Id       { get; set; }
            public Movie  Movie    { get; set; }
            public int    Capacity { get; set; }

            public ScreeningState When(V1.SeatReserved evt)
            {
                Seats.Add((evt.Row, evt.Seat));
                Capacity--;
                return this;
            }

            public List<(int Row, int Seat)> Seats { get; } = new List<(int Row, int Seat)>();
        }

        public void ReserveSeat(in int row, in int seat, string userId, DateTimeOffset when)
        {
            Apply(new V1.SeatReserved
            {
                ScreeningId = State.Id,
                Seat        = seat,
                Row         = row,
                SeatsLeft   = State.Capacity - 1,
                ReservedBy  = userId,
                ReservedAt  = when
            });
        }
    }
}