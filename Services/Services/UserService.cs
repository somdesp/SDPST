using AutoMapper;
using Domain.Entity;
using Infra.Interface;
using Services.Helpers;
using Services.Interface;
using System;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private Encryption encrypted = new Encryption();
        private readonly IMapper _mapper;



        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;

        }

        public async Task<bool> CreateUserAsync(object model, IValidationDictionary validation)
        {
            User user;

            try
            {
                user = _mapper.Map<User>(model);
                user.Password = encrypted.HashHmac(user.Email + "OPTIMUS@@ECM", user.Password);

                //verifica se usuario existe
                if (await _userRepository.ValidUserAsync(user))
                {
                    validation.AddError("errors", "Usuário já existe");
                    return false;
                }   
                await _userRepository.CreateUserAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return true;
        }
    }
}
