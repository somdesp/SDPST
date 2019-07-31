using Domain.Entity;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(object model, IValidationDictionary validation);
        Task<bool> CreateUserFacebookAsync(object model);
        Task<User> GetUser(int id);
    }
}
