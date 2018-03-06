using Autofac;


namespace Djm.OGame.Web.Api.Autofac
{
    public class UtilsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
           // builder.RegisterType<OgClient>().As<IOgClient>().SingleInstance();
        }
    }
}