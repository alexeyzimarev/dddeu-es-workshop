using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace EventSourcing.Projections
{
    public class ScheduledScreenings
    {
        [BsonId]
        public string TheaterId { get; set; }
        
        public List<Screening> Screenings { get; set; } = new List<Screening>();
        
        public class Screening
        {
            public string MovieId { get; set; }
            public string MovieName { get; set; }
            public DateTimeOffset ScheduledAt { get; set; }
            public int DurationInMinutes { get; set; }
            public int SeatsLeft { get; set; }
        }
    }
}