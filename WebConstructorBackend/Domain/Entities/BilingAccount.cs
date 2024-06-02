namespace WebConstructorBackend.Domain.Entities
{
    public class BilingAccount
    {
        public Guid ID { get; set; }

        public float Rest { get; set; }

        public Guid UserID { get; set; }
        public User User { get; set; }
    }
}
