using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DerbyTracker.Common.Services
{
    public interface IBoutRunnerService
    {
        void StartBout(Bout bout);
        BoutState GetBoutState(Guid boutId);
        bool IsRunning(Guid boutId);
        List<RunningBout> RunningBouts();
    }

    public class BoutRunnerService : IBoutRunnerService
    {
        private readonly Dictionary<Guid, Bout> _bouts = new Dictionary<Guid, Bout>();
        private readonly Dictionary<Guid, BoutState> _boutStates = new Dictionary<Guid, BoutState>();

        public void StartBout(Bout bout)
        {
            if (_bouts.ContainsKey(bout.BoutId))
            {
                throw new BoutAlreadyRunningException(bout.BoutId);
            }
            _bouts[bout.BoutId] = bout;
            _boutStates[bout.BoutId] = new BoutState() { BoutId = bout.BoutId };
        }

        public BoutState GetBoutState(Guid boutId)
        {
            return _boutStates[boutId];
        }

        public bool IsRunning(Guid id)
        {
            return _bouts.ContainsKey(id);
        }

        public List<RunningBout> RunningBouts()
        {
            var retVal = _bouts.Select(b => new RunningBout { BoutId = b.Key, Name = b.Value.Name }).ToList();
            return retVal;
        }
    }

    public class RunningBout
    {
        public Guid BoutId { get; set; }
        public string Name { get; set; }
    }
}
