using System.Collections.Generic;

namespace DerbyJson
{
    public class Root
    {
        public string Version { get; set; }
        public MetaData Metadata { get; set; }
        public string Type { get; set; }
        public Dictionary<string, Team> Teams { get; set; }
        public Dictionary<string, Period> Periods { get; set; }
        public Ruleset Ruleset { get; set; }
        public Venue Venue { get; set; }
        public string[] Uuid { get; set; }
        public NoteObject[] NoteObjects { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string End_Time { get; set; }
        public League[] Leagues { get; set; }
        public Dictionary<string, Timer> Timers { get; set; }
        public string Tournament { get; set; }
        public string HostLeague { get; set; }
        public Expulsion[] Expulsions { get; set; }
        public string[] Suspensions { get; set; }
        public Signature[] Signatures { get; set; }
        public bool Sanctioned { get; set; }
        public string Association { get; set; }
    }
}
