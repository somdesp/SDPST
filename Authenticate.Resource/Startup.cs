using AutoMapper;
using FluentValidation.AspNetCore;
using Infra.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Authenticate.Resource
{
    public class Startup
    {
        private SymmetricSecurityKey _signingKey;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<IJwtFactory, JwtFactory>();

            //// Get options from app settings
            //var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            //// Configure JwtIssuerOptions
            //services.Configure<JwtIssuerOptions>(options =>
            //{
            //    options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
            //    options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
            //    options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            //});

            //var signingConfigurations = new SigningConfigurations();
            //services.AddSingleton(signingConfigurations);

            //var tokenConfigurations = new TokenConfigurations();
            //new ConfigureFromConfigurationOptions<TokenConfigurations>(
            //    Configuration.GetSection("TokenConfigurations"))
            //        .Configure(tokenConfigurations);

            //var AppSettings = new AppSettings();
            //new ConfigureFromConfigurationOptions<AppSettings>(
            //    Configuration.GetSection("AppSettings"))
            //        .Configure(AppSettings);

            //services.AddSingleton(tokenConfigurations);
            //services.AddSingleton(AppSettings);            
            //services.AddScoped<IAuthService, AuthService>();
            //services.AddScoped<IAuthRepository, AuthRepository>();

            //var tokenValidationParameters = new TokenValidationParameters
            //{
            //    ValidateIssuer = true,
            //    ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

            //    ValidateAudience = true,
            //    ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

            //    ValidateIssuerSigningKey = true,
            //    IssuerSigningKey = _signingKey,

            //    RequireExpirationTime = false,
            //    ValidateLifetime = true,
            //    ClockSkew = TimeSpan.Zero
            //};

            //services.AddDbContext<Context>(options => options.UseMySql(
            //    Configuration.GetConnectionString("db_Connection")));

            //services.AddAuthentication(options => {
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddFacebook(facebookOptions =>
            //{
            //    facebookOptions.AppId = "1420621628080924";
            //    facebookOptions.AppSecret = "b73421bcbc99b48bc55d77114f6e6f58";
            //}).AddCookie().AddJwtBearer(configureOptions =>
            //{
            //    configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
            //    configureOptions.TokenValidationParameters = tokenValidationParameters;
            //    configureOptions.SaveToken = true;
            //});

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
            //});

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc();

            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("MyPolicy", configurePolicy =>
                configurePolicy.WithOrigins("http://localhost:4200")
                                .AllowAnyMethod()
                                .AllowCredentials()
                                .AllowAnyHeader());
            });




            // Add framework services.
            services.AddDbContext<Context>(options => options.UseMySql(
                Configuration.GetConnectionString("db_Connection")));
            services.AddIdentity<IdentityUser, IdentityRole>()
                   .AddEntityFrameworkStores<Context>()
                   .AddDefaultTokenProviders();

            //services.AddAuthorization(auth =>
            //{
            //    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
            //        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            //        .RequireAuthenticatedUser().Build());
            //});

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthentication(options =>
            {
                options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
            }).AddGoogle("Google", options =>
                        {
                            options.CallbackPath = new PathString("/google-callback");
                            options.ClientId = "153019731182-kntiut8vdhu3mdhpcig3aobeasekri1s.apps.googleusercontent.com";
                            options.ClientSecret = "HCf3BXB6OVJksUVRjLSi0pcc";
                            options.Events = new OAuthEvents
                            {
                                OnRemoteFailure = (RemoteFailureContext context) =>
                                {
                                    context.Response.Redirect("/home/denied");
                                    context.HandleResponse();
                                    return Task.CompletedTask;
                                }
                            };
             });


            services.AddAutoMapper();

            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("MyPolicy");

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
}