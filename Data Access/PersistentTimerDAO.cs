using System;

namespace HereticalSolutions.Time
{
    [System.Serializable]
    public class PersistentTimerDAO
    {
        public string ID;

        public ETimerState State;
        
        public bool Repeat;
        
        public DateTime StartTime;

        public TimeSpan Duration;

        public DateTime EstimatedFinishTime;
    }
}