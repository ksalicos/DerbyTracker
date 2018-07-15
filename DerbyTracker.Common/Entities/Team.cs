using System.Collections.Generic;

namespace DerbyTracker.Common.Entities
{
    public class Team
    {
        public string Name { get; set; }
        public IEnumerable<Skater> Roster { get; set; }
    }
}