using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.BindingModels.Pins;
using Djm.OGame.Web.Api.BindingModels.Players;
using Djm.OGame.Web.Api.BindingModels.Scores;
using Djm.OGame.Web.Api.Dal;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.Jobs;
using Djm.OGame.Web.Api.Mvc.ModelBinders;
using Djm.OGame.Web.Api.Mvc.Options;
using Djm.OGame.Web.Api.Services.Authentication;
using Djm.OGame.Web.Api.Services.Mails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OGame.Client.Models;
using Swashbuckle.AspNetCore.Swagger;
using Hangfire;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;
using Player = OGame.Client.Models.Player;





namespace Djm.OGame.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IContainer ApplicationContainer { get; private set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //database
            var connectionString = Configuration.GetConnectionString("OGame");
            services.AddDbContext<OGameContext>(db => db.UseSqlServer(connectionString));

            //hangfire
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("OGame")));

            //automapper
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

            //gzip

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression();

            //configuration
            services.AddOptions();
            services.Configure<PaginationOptions>(Configuration.GetSection("Pagination"));
            services.Configure<TokenOptions>(opt =>
            {
                opt.Issuer = Configuration["Jwt:Issuer"];
                opt.Audience = Configuration["Jwt:Audience"];
                opt.Key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]));
            });
               
            services.ConfigureMailOptions(Configuration);

            //swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",new Info{Title = "Djm.Ogame.Api",Version = "v1"});

                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "Djm.OGame.Web.Api.xml");
                c.IncludeXmlComments(xmlPath);
                
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Name = "Authorization",
                    In = "header"
                });

                c.OperationFilter<PagedOperationFilter>();
                c.OperationFilter<BearerTokenOperationFilter>();
            });

            //JWT
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]))                   
                    };

                });
            
            services.AddMvc(mvc =>
            {
                mvc.ModelBinderProviders.Insert(0, new PageModelBinderProvider());
            });

            services.AddLogging();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Utilisateurs", policy => policy
                    .RequireAuthenticatedUser()
                    .RequireRole(Roles.Utilisateur, Roles.Admin));

                options.AddPolicy("Administrateurs", policy => policy.RequireRole(Roles.Admin));
            });

            //autofac

            var builder = new ContainerBuilder();

            builder.Populate(services);
            
            builder.RegisterAssemblyModules(GetType().Assembly);
            
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IApplicationLifetime appLifeTime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCompression();

            app.UseHangfireDashboard();
            app.UseHangfireServer(new BackgroundJobServerOptions()
            {
                Queues = new[] { HangfireQueues.Email }
            });

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Djm.Ogame.Api"); });

            app.UseAuthentication();

            app.UseMvc();

            appLifeTime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }

    public class PagedOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor.Parameters.All(p => p.ParameterType != typeof(Page)))
                return;

            var names = new[]
            {
                nameof(Page.Current),
                nameof(Page.FirstIndex),
                nameof(Page.Size),
            };

            var parametersToRemove = operation.Parameters.Where(p => names.Contains(p.Name)).ToList();

            foreach (var parameter in parametersToRemove)
                operation.Parameters.Remove(parameter);

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "page",
                Required = false,
                Type = "int",
                Minimum = 1
            });

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "pageSize",
                Required = false,
                Type = "int",
                Minimum = 1
            });
        }
    }


    public class BearerTokenOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (!context.ApiDescription.ActionAttributes().OfType<AuthorizeAttribute>().Any())
                return;
            
            operation.Security = new List<IDictionary<string, IEnumerable<string>>>()
            {
                new Dictionary<string, IEnumerable<string>>{["Bearer"] = new string[0]}
            };
        }
    }
}
