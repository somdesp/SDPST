using Domain.Entity;
using Infra.Data;
using Infra.Interface;
using Microsoft.EntityFrameworkCore;
using System;
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
            catch (Exception e)
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

        public Task<User> GetUser(int id)
        {

            return null;//  _context.Users.Where(use => use.Id.ToString() == id.ToString());
        }
    }
}
