using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Upgrade.Service
{
    public class Program
    {
        private static IConfigurationRoot Host { get; set; }
        public static void Main(string[] args)
        {
            Host = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                                       .AddJsonFile("host.json")
                                       .Build();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseUrls(Host["url"])
            .UseStartup<Startup>();
    }
}
