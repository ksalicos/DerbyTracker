namespace DerbyJson
{
    public class JamEvent : PeriodEvent
    {
        public string Event { get; set; }
        public string Skater { get; set; }
        /// <summary>
        /// A team reference string The team involved in this event, if any. Note that a skater
        /// reference can be sufficient to derive what the team is, but in
        /// some cases there is a need for the team information.
        /// </summary>
        public string Team { get; set; }
        public Timestamp Timestamp { get; set; }
    }
}
