using System;

namespace DerbyTracker.Common.Entities
{
    public class Bout
    {
        public Guid BoutId { get; set; }
        public string Name { get; set; }

        //Ahead of myself
        //public Team LeftTeam { get; set; }
        //public Team RightTeam { get; set; }

        //public RuleSet RuleSet { get; set; }

        public string Venue { get; set; }

        public DateTime AdvertisedStart { get; set; }
        //public DateTime ActualStart { get; set; }
        //public DateTime End { get; set; }

        //public IEnumerable<Official> Officials { get; set; }
    }
}
