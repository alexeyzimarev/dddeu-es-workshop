using System;

namespace EventSourcing.Domain
{
    public static class ScreeningEvents
    {
        public static class V1
        {
            public class ScreeningScheduled
            {
                public string ScreeningId { get; set; }
                public string MovieId { get; set; }
                public string TheaterId { get; set; }
                public DateTimeOffset StartsAt { get; set; }
                public int DurationInMinutes { get; set; }

                public override string ToString()
                    => $"Screening {ScreeningId} scheduled for the movie {MovieId} in the theater {TheaterId} at {StartsAt}";
            }

            public class ScreeningCancelled
            {
                public string ScreeningId { get; set; }
                public string Reason { get; set; }

                public override string ToString()
                    => $"Screening {ScreeningId} cancelled because of {Reason}";
            }

            public class SeatReserved
            {
                public string ScreeningId { get; set; }
                public int Row { get; set; }
                public int Seat { get; set; }
            }
            
            public class SeatReleased
            {
                public string ScreeningId { get; set; }
                public int Row { get; set; }
                public int Seat { get; set; }
            }
        }
    }
}