using Domain.Entity;
using Services.Helpers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IAuthService
    {
        Task<Token> Authenticate(User login);
        Task<User> LoginFacebook(string email);
        Task<ClaimsIdentity> GetClaimsIdentity(User user);
       
    }
}
