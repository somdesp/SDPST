using Domain.Entity;
using Services.Helpers;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IAuthService
    {
        Task<Token> Authenticate(User login);

    }
}
