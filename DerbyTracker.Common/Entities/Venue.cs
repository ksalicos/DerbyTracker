using System;

namespace DerbyTracker.Common.Entities
{
    public class Venue
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
