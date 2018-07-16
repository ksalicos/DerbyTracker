using DerbyTracker.Common.Entities;
using System;

namespace DerbyTracker.Common.Exceptions
{
    public class InvalidBoutPhaseException : Exception
    {
        public BoutPhase Phase { get; set; }

        public InvalidBoutPhaseException(BoutPhase phase)
            : base($"Action could not be completed because bout was in phase: {phase}")
        {
            Phase = phase;
        }
    }
}
