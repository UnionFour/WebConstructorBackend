namespace WebConstructorBackend.Domain.Entities
{
    public class Gym
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public Guid OrganizationID { get; set; }
        public Organization Organization { get; set; }
    }
}
