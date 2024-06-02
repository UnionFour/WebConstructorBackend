namespace WebConstructorBackend.Domain.Entities
{
    public class Couch : User
    {
        public Guid OrganizationID { get; set; }
        public Organization Organization { get; set; }
    }
}
