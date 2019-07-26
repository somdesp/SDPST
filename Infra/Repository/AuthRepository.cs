﻿using Domain.Entity;
using Infra.Data;
using Infra.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        public User AuthUserAsync(User userAuth)
        {
            return  _context.Users.Where(user => user.Email == userAuth.Email.ToLower() && 
                                user.Password == userAuth.Password).FirstOrDefault();
        }


        public Task<User> AuthUserValAsync(User userAuth)
        {
            return _context.Users.SingleOrDefaultAsync(user => user.Email == userAuth.Email.ToLower());
        }


    }
}