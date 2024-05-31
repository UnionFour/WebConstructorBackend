using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public interface ITrainingRepository
    {
        public Training GetTraining(Guid id);

        public Training CreateTraining(Training training);

        public Training UpdateTraining(Guid id, Training training);

        public void DeleteTraining(Guid id);
    }
}
