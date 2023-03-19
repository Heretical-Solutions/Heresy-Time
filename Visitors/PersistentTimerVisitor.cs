using HereticalSolutions.Persistence;

using HereticalSolutions.Time.Factories;

namespace HereticalSolutions.Time.Visitors
{
    public class PersistentTimerVisitor
        : ILoadVisitorGeneric<IPersistentTimer, PersistentTimerDTO>,
          ISaveVisitorGeneric<IPersistentTimer, PersistentTimerDTO>
    {
        public bool Load(
            PersistentTimerDTO DTO,
            out IPersistentTimer value)
        {
            value = TimersFactory.BuildPersistentTimer(
                DTO.ID,
                DTO.DefaultDurationSpan);
            
            ((ITimerWithState)value).SetState(DTO.State);

            ((IPersistentTimerContext)value).StartTime = DTO.StartTime;
            
            ((IPersistentTimerContext)value).EstimatedFinishTime = DTO.EstimatedFinishTime;
            
            ((IPersistentTimerContext)value).SavedProgress = DTO.SavedProgress;

            ((IPersistentTimerContext)value).CurrentDurationSpan = DTO.CurrentDurationSpan;
            
            value.Accumulate = DTO.Accumulate;

            value.Repeat = DTO.Repeat;

            return true;
        }

        public bool Save(
            IPersistentTimer value,
            out PersistentTimerDTO DTO)
        {
            DTO = new PersistentTimerDTO
            {
                ID = value.ID,
                State = value.State,
                StartTime = ((IPersistentTimerContext)value).StartTime,
                EstimatedFinishTime = ((IPersistentTimerContext)value).EstimatedFinishTime,
                SavedProgress = ((IPersistentTimerContext)value).SavedProgress,
                Accumulate = value.Accumulate,
                Repeat = value.Repeat,
                CurrentDurationSpan = value.CurrentDurationSpan,
                DefaultDurationSpan = value.DefaultDurationSpan
            };

            return true;
        }
    }
}