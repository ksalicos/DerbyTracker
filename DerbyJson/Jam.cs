namespace DerbyJson
{
    public class Jam : PeriodEvent
    {
        public int Number { get; set; }
        public Timestamp Timestamp { get; set; }
        public int Duration { get; set; }
        public JamEvent[] Events { get; set; }
        public NoteObject[] Notes { get; set; }
    }
}
