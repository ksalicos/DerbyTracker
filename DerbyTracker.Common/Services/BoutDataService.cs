using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DerbyTracker.Common.Services
{
    public interface IBoutDataService
    {
        BoutData Load(Guid id);
        List<BoutListItem> List();
        void Save(BoutData boutData);
    }

    public class BoutDataService : IBoutDataService
    {
        private static IConfiguration Configuration { get; set; }

        private static string _boutDataPath;

        public BoutDataService(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _boutDataPath = $"{hostingEnvironment.ContentRootPath}/{Configuration["BoutDataPath"]}";

        }

        public List<BoutListItem> List()
        {
            var path = $"{_boutDataPath}/boutdata.json";

            if (!Directory.Exists(_boutDataPath))
            { Directory.CreateDirectory(_boutDataPath); }

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var boutData = JsonConvert.DeserializeObject<List<BoutListItem>>(json);
                return boutData;
            }

            File.WriteAllLines(path, new[] { "[]" });
            return new List<BoutListItem>();
        }

        public BoutData Load(Guid id)
        {
            var path = $"{_boutDataPath}/{id}.json";
            if (!File.Exists(path))
            { throw new BoutNotFoundException(id); }

            var json = File.ReadAllText(path);
            var boutData = JsonConvert.DeserializeObject<BoutData>(json);
            return boutData;
        }

        public void Save(BoutData boutData)
        {
            if (boutData.BoutId == Guid.Empty)
            { boutData.BoutId = Guid.NewGuid(); }
            //TODO: For testing purposes, all bouts are set to the current WFTDA rules on save          
            //if (bout.RuleSet == null)
            { boutData.RuleSet = RuleSet.WFTDA; }

            var filePath = $"{_boutDataPath}/{boutData.BoutId}.json";
            var boutJson = JsonConvert.SerializeObject(boutData);
            File.WriteAllText(filePath, boutJson);

            var list = List();
            var entry = list.FirstOrDefault(x => x.Id == boutData.BoutId);

            if (entry == null)
            {
                list.Add(new BoutListItem
                {
                    Id = boutData.BoutId,
                    Name = boutData.Name,
                    TimeStamp = DateTime.Now
                });
            }
            else
            {
                entry.TimeStamp = DateTime.Now;
                entry.Name = boutData.Name;
            }

            var path = $"{_boutDataPath}/boutdata.json";
            var listJson = JsonConvert.SerializeObject(list);
            File.WriteAllText(path, listJson);
        }
    }

    public class BoutListItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
