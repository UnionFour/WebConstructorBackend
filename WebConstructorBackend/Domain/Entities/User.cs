namespace WebConstructorBackend.Domain.Entities
{
    public class User
    {
        public Guid ID { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string passHash { get; set; }

        public string AvatarPicturePath { get; set; }

        public bool IsAuthor { get; set; }

        public bool IsCouch { get; set; }

        public BilingAccount BilingAccount { get; set; }
    }
}
