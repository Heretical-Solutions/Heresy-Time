namespace HereticalSolutions.Time
{
    [System.Serializable]
    public class RuntimeTimerDTO
    {
        public string ID;

        public ETimerState State;

        public float CurrentTimeElapsed;
        
        public bool Accumulate;
        
        public bool Repeat;
        
        public float CurrentDuration;
        
        public float DefaultDuration;
    }
}