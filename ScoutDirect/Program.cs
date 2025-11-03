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
                    //var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    //if (!environmentName.Equals(ENVIRONMENT_DEVELOPMENT, StringComparison.OrdinalIgnoreCase))
                    //{
                    //    webBuilder.UseKestrel(kestrelOptions =>
                    //    {
                    //        kestrelOptions.ConfigureHttpsDefaults(httpsOptions =>
                    //        {
                    //            httpsOptions.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
                    //        });
                    //    });
                    //}

                    webBuilder.UseSentry(o =>
                    {
                        o.Dsn = "https://ad84f51dd588fd9eb4d161182361c90d@o4510284610207744.ingest.us.sentry.io/4510284611584001";
                        // When configuring for the first time, to see what the SDK is doing:
                        o.Debug = true;
                    }).UseStartup<Startup>(); ;
                });
    }
}
