using Domain.Entity;
using Domain.ViewModel;
using Services.Helpers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.Auth.Service.Interface
{
    public interface IAuthService
    {
        Task<Token> Authenticate(LoginViewModel login);
        Task<ClaimsIdentity> GetClaimsIdentity(LoginViewModel user);

    }
}
