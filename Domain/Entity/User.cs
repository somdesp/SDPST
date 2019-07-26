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

        // Extended Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? FacebookId { get; set; }
        public string PictureUrl { get; set; }



    }
}
