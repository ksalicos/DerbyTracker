using DerbyTracker.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DerbyTracker.Common.Services.Mocks
{
    public class MockBoutDataService : IBoutDataService
    {
        public static Guid EmptyBoutId = Guid.Empty;
        private readonly List<BoutListItem> _boutList = new List<BoutListItem> { new BoutListItem { Id = EmptyBoutId } };
        private readonly Dictionary<Guid, BoutData> _bouts = new Dictionary<Guid, BoutData>
        {
            {
                EmptyBoutId, new BoutData{BoutId = EmptyBoutId, Name = "Empty", RuleSet = RuleSet.WFTDA}
            }
        };

        public List<BoutListItem> List() => _boutList;

        public BoutData Load(Guid id) => _bouts.ContainsKey(id) ? _bouts[id] : null;

        public void Save(BoutData boutData)
        {
            _bouts[boutData.BoutId] = boutData;
            var entry = _boutList.FirstOrDefault(x => x.Id == boutData.BoutId);

            if (entry == null)
            {
                _boutList.Add(new BoutListItem
                {
                    Id = boutData.BoutId,
                    Name = $"New Bout {DateTime.Now}",
                    TimeStamp = DateTime.Now
                });
            }
            else
            { entry.TimeStamp = DateTime.Now; }
        }
    }
}
