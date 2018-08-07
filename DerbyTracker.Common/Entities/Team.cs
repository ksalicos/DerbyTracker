using System.Collections.Generic;

namespace DerbyTracker.Common.Entities
{
    public class Team
    {
        public string Name { get; set; }
        public string Color { get; set; }

        public List<Skater> Roster { get; set; }
    }
}