using Domain.Entity;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(object model, IValidationDictionary validation);

        Task<IEnumerable<User>> GetUserAsync(int id, IValidationDictionary validation);

    }
}
