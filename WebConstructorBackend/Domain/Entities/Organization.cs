namespace WebConstructorBackend.Domain.Entities
{
    public class Organization
    {
        public Guid ID { get; set; }

        public string Description { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public ICollection<Gym> Gyms { get; set; } = new List<Gym>();

        public ICollection<Couch> Couches { get; set; } = new List<Couch>();

        public ICollection<SportEvent> Events { get; set; } = new List<SportEvent>();

        public Guid OrganizatorID { get; set; }
        public Organizator Organizator { get; set; }
    }
}
