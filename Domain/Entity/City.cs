namespace Domain.Entity
{
    public class City:Base
    {
        public City()
        {

        }

        public State State { get; set; }
        public int StateId { get; set; }

    }
}
