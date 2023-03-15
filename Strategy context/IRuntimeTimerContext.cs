namespace HereticalSolutions.Time
{
    public interface IRuntimeTimerContext : ITimerWithState
    {
        float TimeElapsed { get; set; }

        float CurrentDuration { get; set; }

        float DefaultDuration { get; }
    }
}