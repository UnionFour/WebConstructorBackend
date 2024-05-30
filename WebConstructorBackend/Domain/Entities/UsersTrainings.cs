namespace WebConstructorBackend.Domain.Entities
{
    public class UsersTrainings
    {
        public Guid ID { get; set; }

        public Guid UserID { get; set; }

        public Guid TrainingID { get; set; }

        public bool IsPaied { get; set; }

        public float Cost { get; set; }
    }
}
