using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApplicationService.API;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DemoApp.Web
{
    public class Program
    {
        private static IWebHost builder;

        public static void Main(string[] args)
        {
            RunWebsite(args);
        }

        public static void RunWebsite(string[] args)
        {
            builder = CreateWebHostBuilder(args).Build();

            builder.Run();
        }



        public static async Task StopWebsite()
        {
            await builder.StopAsync();
        }





        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
