namespace WebConstructorBackend.Domain.Entities
{
    public class Training
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Room { get; set; }

        public DateTime TrainingStart { get; set; }

        public DateTime TrainingEnd { get; set; }

        public float Cost { get; set; }
        public ICollection<UsersTrainings> UsersTrainings { get; set; } = new List<UsersTrainings>();

        public Guid CouchID { get; set; }
        public User Couch { get; set; }

        public Guid OrganizationID { get; set; }
        public Organization Organization { get; set; }
    }
}
