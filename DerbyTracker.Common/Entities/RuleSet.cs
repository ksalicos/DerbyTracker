﻿namespace DerbyTracker.Common.Entities
{
    public class RuleSet
    {
        public int NumberOfPeriods { get; set; }
        public int JamDurationSeconds { get; set; }
        public int LineupDurationSeconds { get; set; }
        public int PeriodDurationSeconds { get; set; }

        public static readonly RuleSet WFTDA = new RuleSet
        {
            NumberOfPeriods = 2,
            JamDurationSeconds = 120,
            LineupDurationSeconds = 30,
            PeriodDurationSeconds = 1800
        };
    }
}