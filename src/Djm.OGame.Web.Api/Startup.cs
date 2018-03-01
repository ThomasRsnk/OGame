using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Pins;
using Djm.OGame.Web.Api.BindingModels.Players;
using Djm.OGame.Web.Api.BindingModels.Scores;
using Djm.OGame.Web.Api.Dal;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.Dal.Repositories.Pin;
using Djm.OGame.Web.Api.Dal.Repositories.Player;
using Djm.OGame.Web.Api.Dal.Repositories.Univers;
using Djm.OGame.Web.Api.Dal.Services;
using Djm.OGame.Web.Api.Services.OGame.Alliances;
using Djm.OGame.Web.Api.Services.OGame.Pictures;
using Djm.OGame.Web.Api.Services.OGame.Pins;
using Djm.OGame.Web.Api.Services.OGame.Players;
using Djm.OGame.Web.Api.Services.OGame.Scores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OGame.Client;
using OGame.Client.Models;
using Player = OGame.Client.Models.Player;


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

                cfg.CreateMap<Player, PlayerListItemBindingModel>()
                    .ForMember(dest => dest.ProfilePicUrl, opt => opt.Ignore());

            });
            services.AddMvc();
            services.AddLogging();

            services.AddSingleton<IOgClient, OgClient>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IPinsService, PinsService>();
            services.AddScoped<IPlayersService, PlayerService>();
            services.AddScoped<IAlliancesService, AllianceService>();
            services.AddScoped<IScoresService, ScoreService>();
            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<IPinRepository, PinRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IUniversRepository, UniversRepository>();

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
