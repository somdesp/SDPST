using System.Collections.Generic;

namespace Domain.Entity
{
   public class State :Base
    {
        public State()
        {

        }

        public Country Country { get; set; }
        public int CountryId { get; set; }
        public List<City> Cities { get; set; }
    }
}
