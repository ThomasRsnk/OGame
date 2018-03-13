using System;
using Autofac;
using Djm.OGame.Web.Api.Dal;
using Djm.OGame.Web.Api.Dal.Services;
using Microsoft.EntityFrameworkCore;

namespace Djm.OGame.Web.Api.Autofac
{
    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OGameContext>()
                .AsSelf()
                .As<DbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(s => s.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}