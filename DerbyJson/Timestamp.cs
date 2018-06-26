namespace DerbyJson
{
    public class Timestamp
    {
        public string Wall { get; set; }
        public int Epoch { get; set; }
        public string Period { get; set; }
        public int Seconds { get; set; }
        public int Jam { get; set; }
        public bool Approximate { get; set; }
        public Timestamp[] Range { get; set; }
    }
}