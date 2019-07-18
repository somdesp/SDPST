using Domain.Entity;
using Infra.Interface;
using Services.Helpers;
using Services.Interface;
using System;

namespace Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private Encryption encrypted = new Encryption();


        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public dynamic InserirUsuario(User user)
        {

            try
            {
                user.Password = encrypted.HashHmac(user.Email+"OPTIMUS@@ECM", user.Password);
                _userRepository.InserirUsuario(user);
            }
            catch
            {
                throw new Exception("Dados invalidos");
            }

            return true;
        }
    }
}
