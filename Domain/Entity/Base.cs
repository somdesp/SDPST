using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public  class Base
    {
        [Key]
        public int  Id { get; set; }
        public string Name { get; set; }

    }
}
