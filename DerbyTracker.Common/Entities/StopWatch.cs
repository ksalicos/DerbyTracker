using System;
using System.Collections.Generic;
using System.Linq;

namespace DerbyTracker.Common.Entities
{
    public class StopWatch
    {
        //This must always have one value or Elapsed will fail.
        private readonly List<TimeSpan> _runningTimes = new List<TimeSpan> { TimeSpan.FromSeconds(0) };
        public DateTime LastStarted = DateTime.Now;
        public bool Running { get; set; }
        public TimeSpan Elapsed => Running ? _runningTimes.Aggregate((a, b) => a + b) + (DateTime.Now - LastStarted)
            : _runningTimes.Aggregate((a, b) => a + b);

        //This is used in the client to calculate running times.
        public int ElapsedMs => (int)_runningTimes.Aggregate((a, b) => a + b).TotalMilliseconds;

        public void Nudge(TimeSpan amount)
        {
            _runningTimes.Add(amount);
        }

        public void Start()
        {
            if (Running) return;
            LastStarted = DateTime.Now;
            Running = true;
        }

        public void Stop()
        {
            if (!Running) return;
            _runningTimes.Add(DateTime.Now - LastStarted);
            Running = false;
        }

        public void Clear()
        {
            _runningTimes.Clear();
            _runningTimes.Add(TimeSpan.Zero);
            Running = false;
        }
    }
}
