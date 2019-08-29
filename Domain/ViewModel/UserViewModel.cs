using System;

namespace Domain.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateRegister { get; set; }
        public bool Status { get; set; }

    }
}
