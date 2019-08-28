using System;

namespace Domain.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateRegister { get; set; }
        public bool Status { get; set; }

    }
}
