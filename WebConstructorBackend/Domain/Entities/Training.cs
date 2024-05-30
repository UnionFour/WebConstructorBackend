namespace WebConstructorBackend.Domain.Entities
{
    public class Training
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Room { get; set; }

        public Guid CoachID { get; set; }

        public DateTime TrainingStart { get; set; }

        public DateTime TrainingEnd { get; set; }

        public float Cost { get; set; }
    }
}
