namespace DerbyJson
{
    public class TimeOut
    {
        /// <summary>
        /// The document lists this as "timeout", I am assuming that to be a typo. -KS
        /// </summary>
        public string Team { get; set; }
        public NoteObject[] Notes { get; set; }
        public string Injury { get; set; }
        public int Duration { get; set; }
        public Timestamp Timestamp { get; set; }
        public string Review { get; set; }
        public string Resolution { get; set; }
        public bool Retained { get; set; }
    }
}
