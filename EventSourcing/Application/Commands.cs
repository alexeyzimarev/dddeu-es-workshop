using System;

namespace EventSourcing.Application
{
    public static class ScreeningCommands
    {
        public static class V1
        {
            public class ScheduleScreening
            {
                public string ScreeningId { get; set; }
                public string MovieId { get; set; }
                public string TheaterId { get; set; }
                public DateTimeOffset StartsAt { get; set; }
            }

            public class ReserveSeat
            {
                public string ScreeningId { get; set; }
                public int Row { get; set; }
                public int Seat { get; set; }
            }
        }
    }
}