using Autofac;
using Djm.OGame.Web.Api.Services.Mails;
using Microsoft.Extensions.Options;


namespace Djm.OGame.Web.Api.Autofac
{
    public class MailModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<RazorViewToStringRenderer>();

            builder
                .RegisterType<MailService>()
                .As<IMailService>();

            builder
                .Register(ctx => ctx.Resolve<IOptions<MailOptions>>().Value.Smtp)
                .AsSelf();

            builder
                .RegisterType<DefaultSmtpClient>()
                .As<ISmtpClient>();
        }
    }
}