using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(object model, IValidationDictionary validation);

        Task<bool> CreateUserFacebookAsync(object model);

    }
}
