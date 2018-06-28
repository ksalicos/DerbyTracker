using DerbyTracker.Common.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DerbyTracker.Common.Services
{
    public interface IBoutService
    {
        Bout Load(string id);
        IEnumerable<BoutListItem> List();
        void Save(Bout bout);
        Bout Create();
    }

    public class BoutService : IBoutService
    {
        //private readonly IHostingEnvironment _hostingEnvironment;
        private static IConfiguration Configuration { get; set; }

        private static string BoutDataPath;

        public BoutService(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            BoutDataPath = $"{hostingEnvironment.ContentRootPath}/{Configuration["BoutDataPath"]}";

        }

        public IEnumerable<BoutListItem> List()
        {
            var path = $"{BoutDataPath}/boutdata.json";

            //TODO: If file doesn't exist, create it.  Path too.

            var json = File.ReadAllText(path);
            var boutData = JsonConvert.DeserializeObject<List<BoutListItem>>(json);
            return boutData;
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

    public class BoutListItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
