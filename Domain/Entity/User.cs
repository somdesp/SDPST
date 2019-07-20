using System;

namespace Domain.Entity
{
    public class User : Base
    {
        public User()
        {

        }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateRegister { get; set; }


        
    }
}
