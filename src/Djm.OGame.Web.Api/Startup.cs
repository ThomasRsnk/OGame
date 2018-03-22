using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Djm.OGame.Web.Api.AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.Dal;
using Djm.OGame.Web.Api.Jobs;
using Djm.OGame.Web.Api.Mvc.Authorizations;
using Djm.OGame.Web.Api.Mvc.ModelBinders;
using Djm.OGame.Web.Api.Mvc.Options;
using Djm.OGame.Web.Api.Services.Authentication;
using Djm.OGame.Web.Api.Services.Mails;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Djm.OGame.Web.Api.Dal.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;


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

            //identity

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<OGameContext>()
                .AddDefaultTokenProviders();


            services.AddAuthentication().AddTwitter(twitterOptions =>
            {
                twitterOptions.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
                twitterOptions.ConsumerSecret =Configuration["Authentication:Twitter:ConsumerSecret"];
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
                
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
               
                options.LoginPath = "/Account/Login";
                
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });


            //hangfire
            //services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("OGame")));

            //automapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingConfiguration>();
            });

            //gzip

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression();

            //options
            services.AddOptions();
            services.Configure<PaginationOptions>(Configuration.GetSection("Pagination"));
            services.Configure<Mvc.Options.TokenOptions>(opt =>
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

            //Server cache

            services.AddResponseCaching();

            //MVC

            services.AddMvc(mvc =>
            {
                mvc.SslPort = 44316;
                mvc.Filters.Add(new RequireHttpsAttribute());

                mvc.ModelBinderProviders.Insert(0, new PageModelBinderProvider());

                mvc.CacheProfiles.Add("Default",
                    new CacheProfile()
                    {
                        Duration = 10,
                        Location = ResponseCacheLocation.Any
                    });
              });

            services.AddAntiforgery(
                options =>
                {
                    options.Cookie.Name = "_af";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.HeaderName = "X-XSRF-TOKEN";
                }
            );


            //log

            services.AddLogging();

            //Authorization

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Utilisateurs", policy => policy
                    .RequireAuthenticatedUser()
                    .RequireRole(Roles.Utilisateur, Roles.Admin));

                options.AddPolicy("Admin", policy => policy.RequireRole(Roles.Admin));

                options.AddPolicy("EditDeleteArticle",policy => policy.Requirements.Add(new SameAuthorRequirement()));
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
            app.UseResponseCaching();

            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                {
                    Public = true,
                    MaxAge = TimeSpan.FromSeconds(10)
                };
                context.Response.Headers[HeaderNames.Vary] = new string[] { "Accept-Encoding" };

                await next();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           //app.UseRewriter(new RewriteOptions().AddRedirectToHttps());

            app.UseResponseCompression();

//            app.UseHangfireDashboard();
//            app.UseHangfireServer(new BackgroundJobServerOptions()
//            {
//                Queues = new[] { HangfireQueues.Email }
//            });

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Djm.Ogame.Api"); });

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRewriter(new RewriteOptions().AddRedirectToHttps());

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
                Name = "pageLength",
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

    
  
    public class ETagFilter : Attribute, IActionFilter
    {
        private readonly int[] _statusCodes;

        public ETagFilter(params int[] statusCodes)
        {
            _statusCodes = statusCodes;
            if (statusCodes.Length == 0) _statusCodes = new[] { 200 };
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Request.Method == "GET")
            {
                if (_statusCodes.Contains(context.HttpContext.Response.StatusCode))
                {
                    //var content = JsonConvert.SerializeObject(context.Result);

                    var etag = ETagGenerator.GetETag(context.HttpContext.Request.Path.ToString(), Encoding.UTF8.GetBytes(context.Result.ToString()));

                    if (context.HttpContext.Request.Headers.Keys.Contains("If-None-Match") && context.HttpContext.Request.Headers["If-None-Match"].ToString() == etag)
                    {
                        context.Result = new StatusCodeResult(304);
                    }
                    context.HttpContext.Response.Headers.Add("ETag", new[] { etag });
                }
            }
        }
    }

    public static class ETagGenerator
    {
        public static string GetETag(string key, byte[] contentBytes)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var combinedBytes = Combine(keyBytes, contentBytes);

            return GenerateETag(combinedBytes);
        }

        private static string GenerateETag(byte[] data)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(data);
                var hex = BitConverter.ToString(hash);
                return hex.Replace("-", "");
            }
        }

        private static byte[] Combine(byte[] a, byte[] b)
        {
            var c = new byte[a.Length + b.Length];
            Buffer.BlockCopy(a, 0, c, 0, a.Length);
            Buffer.BlockCopy(b, 0, c, a.Length, b.Length);
            return c;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ThrottleAttribute : ActionFilterAttribute
    {
        public string Name { get; set; }
        public int Seconds { get; set; }
        public string Message { get; set; }

        private static MemoryCache Cache { get; } = new MemoryCache(new MemoryCacheOptions());

        public override void OnActionExecuting(ActionExecutingContext c)
        {
            var key = string.Concat(Name, "-", c.HttpContext.Request.HttpContext.Connection.RemoteIpAddress);

            if (!Cache.TryGetValue(key, out bool entry))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(Seconds));

                Cache.Set(key, true, cacheEntryOptions);
            }
            else
            {
                if (string.IsNullOrEmpty(Message))
                    Message = "You may only perform this action every {n} seconds.";

                c.Result = new ContentResult { Content = Message.Replace("{n}", Seconds.ToString()) };
                c.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
            }
        }
    }
}
