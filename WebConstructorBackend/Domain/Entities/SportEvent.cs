namespace WebConstructorBackend.Domain.Entities
{
    public class SportEvent
    {
        public Guid ID { get; set; }

        public string Name {get; set;}

        public string Description { get; set;}

        public DateTime StartDate { get; set;}

        public Guid GymID { get; set;}
        public Gym Address { get; set;}

        public Guid OrganizationID { get; set;}
        public Organization Organization { get; set;}

        public ICollection<UsersSportEvents> UsersSportEvents { get; set;}
    }
}
