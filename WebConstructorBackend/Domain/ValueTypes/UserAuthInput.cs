using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.ValueTypes
{
    public class UserAuthInput
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public Organization Organization { get; set; }

        public bool IsOrganizator { get; set; }

        public bool IsCouch { get; set; }
    }
}
