using Domain.Entity;
using System.Threading.Tasks;

namespace Infra.Interface
{
    public interface IAuthRepository
    {
        Task<User> AuthUser(User userAuth);
    }
}
