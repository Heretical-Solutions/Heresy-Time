using System;

namespace HereticalSolutions.Time
{
    [System.Serializable]
    public class PersistentTimerDTO
    {
        public string ID;

        public ETimerState State;

        public DateTime StartTime;

        public DateTime EstimatedFinishTime;

        public TimeSpan SavedProgress;
        
        public bool Accumulate;
        
        public bool Repeat;
        
        public TimeSpan CurrentDurationSpan;
        
        public TimeSpan DefaultDurationSpan;
    }
}