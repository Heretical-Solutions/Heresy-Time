using HereticalSolutions.Persistence;

using HereticalSolutions.Time.Factories;

namespace HereticalSolutions.Time.Visitors
{
    public class RuntimeTimerVisitor
        : ILoadVisitorGeneric<IRuntimeTimer, RuntimeTimerDTO>,
          ISaveVisitorGeneric<IRuntimeTimer, RuntimeTimerDTO>
    {
        public bool Load(
            RuntimeTimerDTO DTO,
            out IRuntimeTimer value)
        {
            value = TimersFactory.BuildRuntimeTimer(
                DTO.ID,
                DTO.DefaultDuration);
            
            ((ITimerWithState)value).SetState(DTO.State);

            ((IRuntimeTimerContext)value).CurrentTimeElapsed = DTO.CurrentTimeElapsed;

            ((IRuntimeTimerContext)value).CurrentDuration = DTO.CurrentDuration;
            
            value.Accumulate = DTO.Accumulate;

            value.Repeat = DTO.Repeat;

            return true;
        }

        public bool Save(
            IRuntimeTimer value,
            out RuntimeTimerDTO DTO)
        {
            DTO = new RuntimeTimerDTO
            {
                ID = value.ID,
                State = value.State,
                CurrentTimeElapsed = ((IRuntimeTimerContext)value).CurrentTimeElapsed,
                Accumulate = value.Accumulate,
                Repeat = value.Repeat,
                CurrentDuration = value.CurrentDuration,
                DefaultDuration = value.DefaultDuration
            };

            return true;
        }
    }
}