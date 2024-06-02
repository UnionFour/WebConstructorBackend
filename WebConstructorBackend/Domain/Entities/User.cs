namespace WebConstructorBackend.Domain.Entities
{
    public class User
    {
        public Guid ID { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string passHash { get; set; }

        public string AvatarPicturePath { get; set; }

        public BilingAccount BilingAccount { get; set; }

        public ICollection<UsersTrainings> UsersTrainings { get; set; } = new List<UsersTrainings>();
        public ICollection<UsersSportEvents> UsersSportEvents { get; set; } = new List<UsersSportEvents>();
    }
}
