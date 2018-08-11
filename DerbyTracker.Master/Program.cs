using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net;

namespace DerbyTracker.Master
{
    public class Program
    {
        public static IConfiguration Configuration { get; set; }

        static Program()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        public static void Main(string[] args)
        {


            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(o => { o.Listen(IPAddress.Any, int.Parse(Configuration["Port"])); })
                .UseStartup<Startup>();
    }
}
