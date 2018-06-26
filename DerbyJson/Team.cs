namespace DerbyJson
{
    public class Team
    {
        public string Name { get; set; }
        public string League { get; set; }
        public string Abbreviation { get; set; }
        public Person[] Persons { get; set; }
        public string Level { get; set; }
        public string Date { get; set; }
        public Coloring Color { get; set; }
        public Logo Logo { get; set; }
    }
}