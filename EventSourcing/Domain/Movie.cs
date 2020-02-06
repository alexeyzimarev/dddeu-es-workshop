using System;

namespace EventSourcing.Domain
{
    public struct Movie
    {
        public Movie(string id, int durationInMinutes)
        {
            if (durationInMinutes < 10)
                throw new ArgumentOutOfRangeException(nameof(durationInMinutes));
            
            Id = id;
            DurationInMinutes = durationInMinutes;
        }

        public string Id { get; private set; }
        public int DurationInMinutes { get; private set; }
    }
}