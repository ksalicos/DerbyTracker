using DerbyTracker.Common.Entities;
using System;
using System.Collections.Generic;

namespace DerbyTracker.Common.Services
{
    public interface IBoutService
    {
        Bout Load(string id);
        Dictionary<string, string> List();
        void Save(Bout bout);
        Bout Create();
    }

    public class BoutService : IBoutService
    {
        public Dictionary<string, string> List()
        {
            throw new NotImplementedException();
        }

        public Bout Load(string id)
        {
            throw new NotImplementedException();
        }

        public void Save(Bout bout)
        {
            throw new NotImplementedException();
        }

        public Bout Create()
        {
            throw new NotImplementedException();
        }
    }
}
