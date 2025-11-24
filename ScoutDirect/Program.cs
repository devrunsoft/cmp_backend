using System;
using System.Security.Authentication;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ScoutDirect.Api
{
    public class Program
    {
        const string ENVIRONMENT_DEVELOPMENT = "development";
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureLogging(logging =>
                //{
                //    logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
                //    logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
                //})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


                        webBuilder.UseSentry().UseStartup<Startup>(); ;


                });
    }
}
