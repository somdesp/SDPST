﻿using Infra.Data;
using Infra.Interface;
using Infra.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Services;
using Services.Helpers;
using Services.Interface;

namespace WebAPI
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });



            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);

            var AppSettings = new AppSettings();
            new ConfigureFromConfigurationOptions<AppSettings>(
                Configuration.GetSection("AppSettings"))
                    .Configure(AppSettings);

            services.AddSingleton(tokenConfigurations);
            services.AddSingleton(AppSettings);


            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            //}).AddJwtBearer(bearerOptions =>
            //{
            //    var keyByteArray = Encoding.ASCII.GetBytes(AppSettings.Secret);
            //    var signingKey = new SymmetricSecurityKey(keyByteArray);

            //    bearerOptions.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        IssuerSigningKey = signingKey,
            //        ValidAudience = tokenConfigurations.Audience,
            //        ValidIssuer = tokenConfigurations.Issuer,
            //        ValidateIssuerSigningKey = true,
            //        ValidateLifetime = true,
            //        ClockSkew = TimeSpan.FromMinutes(0)


            //    };
            //});

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddDbContext<Context>(options => options.UseSqlServer(
                Configuration.GetConnectionString("db_Connection")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
