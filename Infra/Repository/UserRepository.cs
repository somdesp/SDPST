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
        private readonly Context _context;
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
            catch (Exception ex)
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
                return await _context.Users.AnyAsync(usr => usr.Email == user.Email && usr.Id != user.Id);
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Retorna Usuarios    
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
        #endregion

        #region AtualizaUsuario
        public async Task<User> EditUserAsync(User userEdit, IValidationDictionary validation)
        {
            int returnValue = 0;
            User user;

            try
            {
                user = await _context.Users.Where(usu => usu.Id == userEdit.Id).SingleAsync();

                user.Name = userEdit.Name;
                user.Status = userEdit.Status;
                user.Email = userEdit.Email;
                if (userEdit.Password != null)
                {
                    user.Password = userEdit.Password;
                }

                returnValue = await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}
