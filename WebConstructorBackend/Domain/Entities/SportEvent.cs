namespace WebConstructorBackend.Domain.Entities
{
    public class SportEvent
    {
        public Guid ID { get; set; }

        public string Name {get; set;}

        public string Description { get; set;}

        public DateTime StartDate { get; set;}

        public string Address { get; set;}

        public List<User> Subscrabers { get; set;}
    }
}
