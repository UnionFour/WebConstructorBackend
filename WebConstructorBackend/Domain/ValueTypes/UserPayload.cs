using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.ValueTypes
{
    public class UserPayload
    {
        public string Id { get; set; }

        public string Login { get; set; }

        public Organization Organization { get; set; }

        public string Token { get; set; }

        public bool IsCouch { get; set; }

        public bool isOrganizator { get; set; }
    }
}
