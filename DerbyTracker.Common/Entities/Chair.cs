using System;

namespace DerbyTracker.Common.Entities
{
    public class Chair
    {
        public Guid Id { get; set; }
        public StopWatch StopWatch = new StopWatch();
        public int SecondsOwed { get; set; } = 30;
        public string Team { get; set; }
        public int Number { get; set; }
        public bool IsJammer { get; set; }
    }
}
