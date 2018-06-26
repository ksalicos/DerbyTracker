namespace DerbyJson
{
    public class League
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string[] Uuid { get; set; }
        public Venue Venue { get; set; }
        public Team[] Teams { get; set; }
        public Logo[] Logos { get; set; }
    }
}