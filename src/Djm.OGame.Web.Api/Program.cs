using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Djm.OGame.Web.Api.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Djm.OGame.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile("secret.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json",true,true)
                .Build();

            BuildWebHost(config).Run();
           
        }

        public static IWebHost BuildWebHost(IConfigurationRoot config) =>
            new WebHostBuilder()
                .UseStartup<Startup>()
                .UseKestrel(opt => opt.ConfigureHttps(config))
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("https://localhost:44316")
                .Build();

    }
}
