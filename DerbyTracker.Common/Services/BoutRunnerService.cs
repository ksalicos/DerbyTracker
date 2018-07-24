using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DerbyTracker.Common.Services
{
    public interface IBoutRunnerService
    {
        void StartBout(BoutData boutData);
        BoutState GetBoutState(Guid boutId);
        bool IsRunning(Guid boutId);
        List<RunningBout> RunningBouts();
    }

    public class BoutRunnerService : IBoutRunnerService
    {
        private readonly Dictionary<Guid, BoutData> _bouts = new Dictionary<Guid, BoutData>();
        private readonly Dictionary<Guid, BoutState> _boutStates = new Dictionary<Guid, BoutState>();

        public void StartBout(BoutData boutData)
        {
            if (_bouts.ContainsKey(boutData.BoutId))
            {
                throw new BoutAlreadyRunningException(boutData.BoutId);
            }
            _bouts[boutData.BoutId] = boutData;
            _boutStates[boutData.BoutId] = new BoutState(boutData);
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
