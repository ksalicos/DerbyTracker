namespace DerbyJson
{
    public class Period
    {
        public Timestamp Timestamp { get; set; }
        public Timestamp End { get; set; }
        public PeriodEvent[] Jams { get; set; }
        public Action[] Actions { get; set; }
        public Error[] Errors { get; set; }
    }
}