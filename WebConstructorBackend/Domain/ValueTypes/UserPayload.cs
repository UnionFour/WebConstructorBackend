namespace WebConstructorBackend.Domain.ValueTypes
{
    public class UserPayload
    {
        public string Id { get; set; }

        public string Login { get; set; }

        public string Token { get; set; }

        public bool IsCouch { get; set; }

        public bool isOrganizator { get; set; }
    }
}
