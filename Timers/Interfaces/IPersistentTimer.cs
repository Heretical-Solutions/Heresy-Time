using System;

namespace HereticalSolutions.Time
{
    public interface IPersistentTimer : ITimer
    {
        #region Timer state

        DateTime StartTime { get; }

        DateTime EstimatedFinishTime { get; }

        #endregion

        #region Countdown and Time elapsed

        TimeSpan CountdownSpan { get; }

        TimeSpan TimeElapsedSpan { get; }

        #endregion

        #region Duration

        TimeSpan CurrentDurationSpan { get; }

        TimeSpan DefaultDurationSpan { get; }

        #endregion

        #region Controls

        void Reset(float duration);

        void Start(float duration);

        #endregion
    }
}