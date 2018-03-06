using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Total.AutoCare.Web.Helpers.Mail;

namespace Total.AutoCare.Web.Options
{
    public class MailOptions
    {
        public const string SectionName = "Mail";

        public string From { get; set; }
        public SmtpOptions Smtp { get; set; }
    }

    public static class MailOptionsHelpers
    {
        public static IServiceCollection ConfigureMailOptions(this IServiceCollection services, IConfiguration config,
            string sectionName = null)
            => services.Configure<MailOptions>(config.GetSection(sectionName ?? MailOptions.SectionName));
    }
}
