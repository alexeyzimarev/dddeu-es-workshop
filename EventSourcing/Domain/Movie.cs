using System;

namespace EventSourcing.Domain
{
    public struct Movie : IEquatable<Movie>
    {
        public Movie(string id, int durationInMinutes)
        {
            if (durationInMinutes < 10)
                throw new ArgumentOutOfRangeException(nameof(durationInMinutes));
            
            Id = id;
            DurationInMinutes = durationInMinutes;
        }

        public string Id { get; }
        public int DurationInMinutes { get; }

        public bool Equals(Movie other) => Id == other.Id && DurationInMinutes == other.DurationInMinutes;

        public override bool Equals(object obj) => obj is Movie other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Id, DurationInMinutes);
    }
}