using HereticalSolutions.Delegates;

namespace HereticalSolutions.Time
{
    public interface IRuntimeTimerContext : ITimerWithState
    {
        float TimeElapsed { get; set; }

        float CurrentDuration { get; set; }

        float DefaultDuration { get; }
        
        IPublisherSingleArgGeneric<ITimer> OnStartAsPublisher { get; }
        
        IPublisherSingleArgGeneric<ITimer> OnFinishAsPublisher { get; }
    }
}