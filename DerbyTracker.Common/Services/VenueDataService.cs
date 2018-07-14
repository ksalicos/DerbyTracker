using DerbyTracker.Common.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DerbyTracker.Common.Services
{
    public interface IVenueDataService
    {
        List<Venue> List();
        void Save(Venue venue);
        void Delete(Guid id);
    }

    public class VenueDataService : IVenueDataService
    {
        private static IConfiguration Configuration { get; set; }
        private static string _boutDataPath;

        public VenueDataService(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _boutDataPath = $"{hostingEnvironment.ContentRootPath}/{Configuration["BoutDataPath"]}";

        }

        public List<Venue> List()
        {
            var path = $"{_boutDataPath}/venuedata.json";

            if (!Directory.Exists(_boutDataPath))
            { Directory.CreateDirectory(_boutDataPath); }

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var venueData = JsonConvert.DeserializeObject<List<Venue>>(json);
                return venueData;
            }

            var list = new List<Venue>();
            SaveList(list);
            return list;
        }

        public void Save(Venue venue)
        {
            var list = List();
            var entry = list.FirstOrDefault(x => x.Id == venue.Id);

            if (entry == null)
            {
                list.Add(venue);
            }
            else
            {
                entry.Name = venue.Name;
                entry.State = venue.State;
                entry.City = venue.City;
            }

            SaveList(list);
        }

        public void Delete(Guid id)
        {
            var list = List();
            var entry = list.FirstOrDefault(x => x.Id == id);
            if (entry == null) return;

            list.Remove(entry);
            SaveList(list);
        }

        private static void SaveList(List<Venue> list)
        {
            var path = $"{_boutDataPath}/venuedata.json";
            var listJson = JsonConvert.SerializeObject(list);
            File.WriteAllText(path, listJson);
        }
    }
}
