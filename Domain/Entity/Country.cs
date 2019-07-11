using System.Collections.Generic;

namespace Domain.Entity
{
    public class Country :Base
    {
        public Country()
        {

        }

        public List<State> States { get; set; }
    }
}
