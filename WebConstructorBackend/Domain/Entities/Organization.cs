namespace WebConstructorBackend.Domain.Entities
{
    public class Organization
    {
        public Guid ID { get; set; }

        public Guid OrganizatorID { get; set; }

        public string Description { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public List<Gym> Gyms { get; set; } = new List<Gym>();
    }
}
