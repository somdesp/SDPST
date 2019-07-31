using Domain.Entity;
using System.Threading.Tasks;

namespace Infra.Interface
{
    public interface IUserRepository
    {
        Task<bool> CreateUserAsync(User user);
        Task<bool> ValidUserAsync(User user);
        Task<User> GetUser(int id);
    }
}
