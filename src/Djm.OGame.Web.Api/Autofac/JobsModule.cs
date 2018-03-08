using Autofac;

namespace Djm.OGame.Web.Api.Autofac
{
    public class JobsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(GetType().Assembly)
                .Where(s => s.Name.EndsWith("Job"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}