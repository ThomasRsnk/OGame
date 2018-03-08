using Autofac;
using Djm.OGame.Web.Api.Controllers;
using Djm.OGame.Web.Api.Services.Authentication;

namespace Djm.OGame.Web.Api.Autofac
{
    public class AuthenticationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JwtFactory>().As<IJwtFactory>();
        }
    }
}