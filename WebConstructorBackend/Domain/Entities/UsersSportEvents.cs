namespace WebConstructorBackend.Domain.Entities
{
    public class UsersSportEvents
    {
        public Guid UserID { get; set; }
        public User User { get; set; }

        public Guid SportEventID { get; set; }
        public SportEvent SportEvent { get; set; }
    }
}
