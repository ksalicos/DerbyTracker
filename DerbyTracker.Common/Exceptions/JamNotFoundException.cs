using System;

namespace DerbyTracker.Common.Exceptions
{
    public class JamNotFoundException : Exception
    {
        public int Period { get; set; }
        public int Jam { get; set; }
        public JamNotFoundException(int period, int jam) : base($"Couldn't find jam {jam} in period {period}.")
        {
            Period = period;
            Jam = jam;
        }
    }
}
