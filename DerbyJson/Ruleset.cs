namespace DerbyJson
{
    public class Ruleset
    {
        public string Version { get; set; }
        public int PeriodCount { get; set; }
        public string Period { get; set; }
        public string Jam { get; set; }
        public string LineUp { get; set; }
        public string TimeOut { get; set; }
        public int TimeOutCount { get; set; }
        public int OfficialReviewCount { get; set; }
        public bool OfficialReviewRetained { get; set; }
        public int OfficialReviewMaximum { get; set; }
        public string Penalty { get; set; }
        public bool Minors { get; set; }
        public int MinorsPerMajor { get; set; }
        public int FoulOut { get; set; }
    }
}