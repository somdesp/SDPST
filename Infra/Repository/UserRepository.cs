using Domain.Entity;
using Infra.Data;
using Infra.Interface;
using System;

namespace Infra.Repository
{
   public class UserRepository: IUserRepository
    {
        private Context _context;
        public UserRepository(Context context)
        {
            _context = context;
        }
        public dynamic InserirUsuario(User user)
        {

            try
            {
                _context.Users.AddAsync(user);
                _context.SaveChangesAsync();

            }
            catch
            {
                throw new Exception("Dados invalidos");
            }
            try
            {
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

            return true;
        }
    }
}
