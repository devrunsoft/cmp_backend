using ScoutDirect.Core.Caching;
using ScoutDirect.Core.Models;
using ScoutDirect.infrastructure;
using ElmahCore;
using ElmahCore.Mvc;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text;
using infrastructure.Data;
//using ScoutDirect.Core.Repositories.Base;
using ScoutDirect.infrastructure.Repository;
using ScoutDirect.Core.Repositories.Base;
using CMPNatural.Application.Handlers;
using CmpNatural.CrmManagment.Model;
using CMPNatural.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ScoutDirect.Api
{
    //public class SignalRContractResolver : IContractResolver
    //{
    //    private readonly Assembly assembly;
    //    private readonly IContractResolver camelCaseContractResolver;
    //    private readonly IContractResolver defaultContractSerializer;

    //    public SignalRContractResolver()
    //    {
    //        defaultContractSerializer = new DefaultContractResolver();
    //        camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
    //        assembly = typeof(Startup).Assembly;
    //    }

    //    public JsonContract ResolveContract(Type type)
    //    {
    //        if (type.Assembly.Equals(assembly))
    //        {
    //            return defaultContractSerializer.ResolveContract(type);
    //        }

    //        return camelCaseContractResolver.ResolveContract(type);
    //    }
    //}

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
            //services.AddCors(); 

            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowOrigin", builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            services.AddSignalR().AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.PropertyNamingPolicy = null;
            });

            services.AddControllers().AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                x.UseMemberCasing();
            });

            //remove
            services.AddControllersWithViews();

            services.AddDbContext<ScoutDBContext>(options => options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")
            ));

            services.AddAutoMapper(typeof(Startup));
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
              typeof(RegisterCompanyHandler).Assembly
              ));


            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.Configure<HighLevelSettings>(Configuration.GetSection("HighLevel"));

            //services.AddScoped<ICallDeferredOrders, CallDeferredOrders>();

            services.RegisterRepositories();

            //// For Identity
            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApiIdentityDbContext>()
            //    .AddDefaultTokenProviders();

            // Adding Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CMPNatural Web API",
                    Description = "Authentication and Authorization in CMPNatural Web API with JWT and Swagger"
                });
                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });

                //swagger.EnableAnnotations();
                //swagger.IncludeXmlComments(filePath);
            });

            services.Configure<CacheConfiguration>(Configuration.GetSection("CacheConfiguration"));
            services.Configure<AppVersionModel>(Configuration.GetSection("AppVersion"));
            //services.Configure<AppConfigModel>(Configuration.GetSection("AppConfig"));

            //For In-Memory Caching
            services.AddMemoryCache();
            services.AddTransient<MemoryCacheService>();
            //services.AddTransient<RedisCacheService>();
            services.AddTransient<Func<CacheTech, ICacheService>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case CacheTech.Memory:
                        return serviceProvider.GetService<MemoryCacheService>();
                    //case CacheTech.Redis:
                    //    return serviceProvider.GetService<RedisCacheService>();
                    default:
                        return serviceProvider.GetService<MemoryCacheService>();
                }
            });

            //services.AddElmah();// in ConfigureServices
            services.AddElmah<XmlFileErrorLog>(options =>
            {
                options.OnPermissionCheck = context => true;//context.User.Identity.IsAuthenticated;
                // ~/elmah is default
                // options.Path = "you_path_here"
                options.LogPath = "~/log"; // OR options.LogPath = "с:\errors";
            });

            services.AddHangfire(x =>
            {
                x.UseMemoryStorage();
                //x.Use(Configuration.GetConnectionString("DBConnection"));
            });

            services.AddHangfireServer();

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            //}

            //if (env.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CMPNatural Web API v1"));
            //}

            app.UseHttpsRedirection();

            //remove
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                        Path.Combine(Directory.GetCurrentDirectory(), "FileContent")),
                RequestPath = "/FileContent"
            });

            app.UseRouting();
            app.UseCors("AllowOrigin");

            //app.UseSignalRQueryStringAuth();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseElmah();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                //endpoints.MapHub<ChatHub>("/chatHub", options =>
                //{
                //    options.Transports = HttpTransportType.WebSockets;
                //    // HttpTransportType.LongPolling;
                //});
                endpoints.MapHangfireDashboard();
                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            GlobalConfiguration.Configuration.UseMemoryStorage();
            //if (!env.IsDevelopment())
            //{
            //    using (var scope = serviceProvider.CreateScope())
            //{
            //    var syncService = scope.ServiceProvider.GetRequiredService<SyncByCrm>();
            //    syncService.sync();
            //}
            //   }

            //Admin_PusherService.ServerKey = Configuration.GetValue<string>("Admin_PusherService_ServerKey");
            //Customer_PusherService.ServerKey = Configuration.GetValue<string>("Customer_PusherService_ServerKey");

            //var settings = new JsonSerializerSettings();
            //settings.ContractResolver = new SignalRContractResolver();
            //var serializer = JsonSerializer.Create(settings);
            //GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);
        }
    }
}
