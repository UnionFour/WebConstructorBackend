namespace WebConstructorBackend.Domain.Entities
{
    public class Organizator : User
    {
        public Guid OrganizationID { get; set; }
        public Organization Organization { get; set; }
    }
}
