using AutoMapper;
using Domain.Entity;
using Domain.Interfaces;
using Infra.Interface;
using Services.Helpers;
using Services.Interface;
using System;
using System.Collections.Generic;
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

        public Task<IEnumerable<User>> GetUserAsync(int id, IValidationDictionary validation)
        {
            return _userRepository.GetUserAsync(id, validation);
        }

        public async Task<User> EditUserAsync(User user, IValidationDictionary validation)
        {
            try
            {
                if (!await ValidUserAsync(user, validation))
                {
                    return null;
                }
                if (user.Password != null)
                {
                    user.Password = encrypted.HashHmac(user.Email + "OPTIMUS@@ECM", user.Password);
                }

                return await _userRepository.EditUserAsync(user, validation);
            }
            catch (Exception)
            {
                return null;
                throw;
            }

        }

        public async Task<bool> ValidUserAsync(User user, IValidationDictionary validation)
        {            
            //verifica se usuario existe
            if (await _userRepository.ValidUserAsync(user))
            {
                validation.AddError("errors", "Usuário já existe");
                return false;
            }
            return true;
        }
    }
}
