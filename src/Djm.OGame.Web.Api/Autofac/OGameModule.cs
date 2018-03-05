using Autofac;
using OGame.Client;
using Module = Autofac.Module;

namespace Djm.OGame.Web.Api.Autofac
{
    public class OGameModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OgClient>().As<IOgClient>().SingleInstance();

            builder.RegisterAssemblyTypes(GetType().Assembly)
                .Where(s => s.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}