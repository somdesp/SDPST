using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(object model, IValidationDictionary validation);
    }
}
