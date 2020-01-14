using Infra.Interface;
using Infra.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Auth.Service.Interface;
using Services.Auth.Service.Service;
using Services.Interface;

namespace SDPST.Infrastructure.CrossCutting.IoC.DependencyInjection
{
    public static class CustomService
    {
        public static void AddCustomService(this IServiceCollection services, IConfiguration configuration)
        {
            #region Injeções Serviços
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            #endregion


            #region Injeções Repositórios
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            #endregion

        }
    }
}
