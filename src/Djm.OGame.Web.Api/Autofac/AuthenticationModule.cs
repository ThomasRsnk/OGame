using Autofac;
using Djm.OGame.Web.Api.Mvc.Authorizations;
using Djm.OGame.Web.Api.Services.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Djm.OGame.Web.Api.Autofac
{
    public class AuthenticationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JwtFactory>().As<IJwtFactory>();

            builder.RegisterType<ArticleAuthorizationHandler>().As<IAuthorizationHandler>();
           
        }
    }
}