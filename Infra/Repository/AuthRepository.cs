using Domain.Entity;
using Infra.Data;
using Infra.Interface;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infra.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly Context _context;

        public AuthRepository(Context context)
        {
            _context = context;
        }

        public async Task<User> AuthUser(User userAuth)
        {
           return userAuth = await _context.Users.SingleOrDefaultAsync(user => user.Name == userAuth.Name && user.Password == userAuth.Password);
        }
    }
}
