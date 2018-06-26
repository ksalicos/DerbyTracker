namespace DerbyJson
{
    public class Pass
    {
        public Timestamp Timestamp { get; set; }
        public int Number { get; set; }
        public int Points { get; set; }
        public string Skater { get; set; }
        public bool Completed { get; set; }
        public GhostPoint[] GhostPoints { get; set; }
    }
}
