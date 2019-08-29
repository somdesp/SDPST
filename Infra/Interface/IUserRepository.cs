using Domain.Entity;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infra.Interface
{
    public interface IUserRepository
    {
        Task<bool> CreateUserAsync(User user);
        Task<bool> ValidUserAsync(User user);
        Task<IEnumerable<User>> GetUserAsync(int id, IValidationDictionary validation);
        Task<User> EditUserAsync(User user, IValidationDictionary validation);


        

    }
}
