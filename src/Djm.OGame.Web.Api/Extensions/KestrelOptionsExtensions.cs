using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;

namespace Djm.OGame.Web.Api.Extensions
{
    public static class KestrelOptionsExtensions
    {
        public static void ConfigureHttps(this KestrelServerOptions options,IConfigurationRoot config)
        {
            var certificateSettings = config.GetSection("certificateSettings");
            var certificateFileName = certificateSettings.GetValue<string>("filename");
            var certificatePassword = certificateSettings.GetValue<string>("password");

            var certificate = new X509Certificate2(certificateFileName, certificatePassword);

            options.AddServerHeader = false;
            options.Listen(IPAddress.Loopback, 44316, listenOptions =>
            {
                listenOptions.UseHttps(certificate);
            });

           

        }
    }
}