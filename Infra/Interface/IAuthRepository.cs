using Domain.Entity;
using System.Threading.Tasks;

namespace Infra.Interface
{
    public interface IAuthRepository
    {
        User AuthUserAsync(User userAuth);
    }
}
