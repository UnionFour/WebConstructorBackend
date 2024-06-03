using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.DTO
{
    public class OrganizationDTO
    {
        public Organization Organization { get; set; }

        public Couch Couch { get; set; }

        public Gym Gym { get; set; }
    }
}
