using System;
using System.Globalization;
using System.Threading.Tasks;
using ALMS.Core;
using ALMS.Data.EFCore;
using ALMS.WebApi.Middlewares;
using AutoMapper;
using Elsa.ApprovalFlowManagement;
using Elsa.NNF.Data.ORM;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;

namespace ALMS.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            ConfigurationRootResolver.LoadApiConfig(Configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddMemoryCache();

            services.UseElsaApprovalFlow(config =>
            {
                config.ApiKey = Guid.Parse(Core.GlobalFields.ApiConfig.ApprovalFlowConfig.ApiKey);
                config.SecretKey = Guid.Parse(Core.GlobalFields.ApiConfig.ApprovalFlowConfig.SecretKey);
                config.ApiUrl = Core.GlobalFields.ApiConfig.ApprovalFlowConfig.ApiUrl;
            });

            #region [ Hosted Services ]

            services.AddHostedService<Service.SessionUpdaterWorkerService>();

            #endregion

            #region [ Load TR Language ]

            var cultureInfo = new CultureInfo("tr-TR");
            cultureInfo.NumberFormat.CurrencySymbol = "₺";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            #endregion

            services.AddControllers();

            #region [ Entity Repository ]
            services.UseElsaEntityRepository(cfg =>
            {
                cfg.RepositoryFactoryBuilder = new EFCoreBuilder<ApiContext>();
                cfg.ErrorMessageLanguageCode = LanguageCode.Tr;
                cfg.DisableEntityCaching = true;
            });

            #endregion

            #region [ AddMvc ]
            services.AddMvcCore(options =>
            {
                options.EnableEndpointRouting = false;
            })
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddAuthorization()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());

                });

            #endregion

            #region [ AutoMapper ]

            services.AddAutoMapper(typeof(DomainProfile));

            #endregion

            #region [ JWT ]
            var jwtConfig = Core.GlobalFields.ApiConfig.JwtTokenConfig;
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidAudience = jwtConfig.ValidAudience,
                        ValidateAudience = jwtConfig.ValidateAudience,
                        ValidIssuer = jwtConfig.ValidIssuer,
                        ValidateIssuer = jwtConfig.ValidateIssuer,
                        ValidateLifetime = jwtConfig.ValidateLifetime,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtConfig.IssuerSigningKey)),
                        ValidateIssuerSigningKey = jwtConfig.ValidateIssuerSigningKey,
                        ClockSkew = TimeSpan.Zero
                    };

                    //JWT eventlarının yakalandığı yerdir.
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Query.TryGetValue("token", out StringValues token))
                            {
                                context.Token = token.ToString();
                            }
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = _ =>
                        {
                            Console.WriteLine($"JWT Exception:{_.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = _ =>
                        {
                            Console.WriteLine($"JWT Login Success:{ _.Principal.Identity}");
                            return Task.CompletedTask;
                        },

                    };
                });
            #endregion

            #region [ Cors ]
            var allowedHosts = GetAllowedHostsFromConfig();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(allowedHosts ?? new[] { "*" })
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    // .AllowAnyOrigin()
                    .AllowCredentials());
            });

            #endregion

            #region [ Swagger ]


            // services.AddDbContext<ApiContext>();

            if (!Core.GlobalFields.ApiConfig.IsProduction)
            {

                services.AddSwaggerGen(swagger =>
                {
                    swagger.SwaggerDoc("v1", new OpenApiInfo { Title = $"{Core.GlobalFields.ApiConfig.AppName} - API", Version = "v1", Contact = new OpenApiContact { Name = "Elsa Bilişim A.Ş." } });
                    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    });
                    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement {
                        {
                            new OpenApiSecurityScheme {
                                Reference = new OpenApiReference {
                                    Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    });
                });
                // services.AddOdataSwaggerSupport();
            }

            #endregion

            services.AddDependencyInjects();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            MappingExtensions.Configure(mapper);

            #region [ Swagger ]

            if (!Core.GlobalFields.ApiConfig.IsProduction)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Core.GlobalFields.ApiConfig.AppName} Service - v1");
                });
            }

            #endregion

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            #region [ Middlewares ]

            app.UseExceptionMiddleware();
            app.UseUserIdentityAuthentication();

            #endregion

            // app.UseAuthorization ();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMvc();
        }

        private string[] GetAllowedHostsFromConfig()
        {
            var allowedHosts = Core.GlobalFields.ApiConfig.AllowedHosts;
            if (string.IsNullOrEmpty(allowedHosts))
                return null;
            return allowedHosts.Split(";");
        }

    }
}
