namespace WebConstructorBackend.Domain.Entities
{
    public class UsersTrainings
    {
        public bool IsPaied { get; set; } = false;

        public Guid UserID { get; set; }
        public User User { get; set; }

        public Guid TrainingID { get; set; }
        public Training Training { get; set; }
    }
}
