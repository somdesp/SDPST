using Domain.Entity;
using Domain.Interfaces;
using Infra.Data;
using Infra.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infra.Repository
{
    public class UserRepository : IUserRepository
    {
        private Context _context;
        public UserRepository(Context context)
        {
            _context = context;
        }

        #region Insere Usuario
        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        #endregion

        #region Verifica se existe usuario
        public async Task<bool> ValidUserAsync(User user)
        {

            try
            {
                return await _context.Users.AnyAsync(usr => usr.Email == user.Email);
            }
            catch
            {
                return false;
            }
        }
        #endregion

        public async Task<IEnumerable<User>> GetUserAsync(int id, IValidationDictionary validation)
        {

            return await Task.Run(() =>
            {
                List<User> usuario = new List<User>();

                try
                {
                    if (id == 0)
                    {
                        return _context.Users.ToList();
                    }

                    usuario = _context.Users.Where(u => u.Id == id).ToList();

                    if (usuario.Count == 0)
                    {
                        validation.AddError("errors", "Usuário não existe");
                        return null;
                    }

                    return usuario;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            });
        }


    }
}
