namespace WebConstructorBackend.Domain.Entities
{
    public class Organization
    {
        public Guid ID { get; set; }

        public string Description { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public List<Gym> Gyms { get; set; } = new List<Gym>();

        public List<User> Couches { get; set; } = new List<User>();

        public Guid OrganizatorID { get; set; }
        public User Organizator { get; set; }
    }
}
