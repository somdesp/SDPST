using Domain.Entity;

namespace Infra.Interface
{
    public interface IUserRepository
    {
        dynamic InserirUsuario(User user);
    }
}
