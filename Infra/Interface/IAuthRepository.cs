using Domain.Entity;
using Domain.ViewModel;
using System.Threading.Tasks;

namespace Infra.Interface
{
    public interface IAuthRepository
    {
        User AuthUserAsync(LoginViewModel userAuth);
    }
}
