using System;

namespace DerbyTracker.Common.Entities
{
    public class BoutData
    {
        public Guid BoutId { get; set; }
        public string Name { get; set; }

        public Team Left { get; set; }
        public Team Right { get; set; }

        public RuleSet RuleSet { get; set; }

        public Venue Venue { get; set; }

        public DateTime AdvertisedStart { get; set; }

        //public IEnumerable<Official> Officials { get; set; }
    }
}
