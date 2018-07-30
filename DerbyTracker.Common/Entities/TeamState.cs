namespace DerbyTracker.Common.Entities
{
    public class TeamState
    {
        public TeamState(RuleSet rules)
        {
            TimeOutsRemaining = rules.TimeOutsPerTeam;
            OfficialReviews = rules.OfficialReviewsPerTeam;
        }

        public int TimeOutsRemaining { get; set; }
        public int OfficialReviews { get; set; }
    }
}
