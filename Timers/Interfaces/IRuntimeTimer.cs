namespace HereticalSolutions.Time
{
    public interface IRuntimeTimer : ITimer
    {
        #region Countdown and Time elapsed

        float Countdown { get; }

        float TimeElapsed { get; }

        #endregion

        #region Duration

        float CurrentDuration { get; }

        float DefaultDuration { get; }

        #endregion

        #region Controls

        void Reset(float duration);

        void Start(float duration);

        #endregion
    }
}