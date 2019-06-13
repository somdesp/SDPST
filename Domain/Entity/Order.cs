using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DateOrder { get; set; }
        public User User { get; set; }
    }
}
