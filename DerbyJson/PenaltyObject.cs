namespace DerbyJson
{
    public class PenaltyObject
    {
        public Timestamp Timestamp { get; set; }
        public string Skater { get; set; }
        public string Penalty { get; set; }
        public bool Rescinded { get; set; }
        public Involved[] Involved { get; set; }
        public string Cue { get; set; }
    }
}
