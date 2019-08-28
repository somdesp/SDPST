using Domain.Entity;
using Services.Helpers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.Auth.Service.Interface
{
    public interface IAuthService
    {
        Task<Token> Authenticate(User login);
        Task<ClaimsIdentity> GetClaimsIdentity(User user);

    }
}
