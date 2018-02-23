using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Pins;
using Djm.OGame.Web.Api.BindingModels.Players;
using Djm.OGame.Web.Api.BindingModels.Scores;
using Djm.OGame.Web.Api.Dal;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.Dal.Repositories;
using Djm.OGame.Web.Api.Dal.Services;
using Djm.OGame.Web.Api.Services.Pictures;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OGame.Client;
using OGame.Client.Models;

namespace Djm.OGame.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("OGame");
            services.AddDbContext<OGameContext>(db => db.UseSqlServer(connectionString));

            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<Player, PlayerDetailsBindingModel>()
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.Status.ToString().Replace("_"," ")));
                cfg.CreateMap<Position, PositionsBindingModel>()
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TypeC.ToString().Replace("_"," ")));

                cfg.CreateMap<PinCreateBindingModel, Pin>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore());
            });
            services.AddMvc();
            services.AddLogging();

            services.AddScoped<IOgClient, OgClient>();
            services.AddScoped<IPinRepository, PinRepository>();
            services.AddScoped<IPicture, PictureHandler>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseMvc();
        }
    }
}
