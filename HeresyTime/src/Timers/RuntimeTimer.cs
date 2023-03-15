using System;

namespace HereticalSolutions.Time.Timers
{
    public class RuntimeTimer
        : IRuntimeTimer,
          ITickable
    {
        private const float EPSILON = 0.0001f;
        
        private float timeElapsed;

        private float currentDuration;

        private float defaultDuration;

        public RuntimeTimer(
            string id,
            float defaultDuration)
        {
            ID = id;

            State = ETimerState.INACTIVE;

            timeElapsed = 0f;

            this.currentDuration = this.defaultDuration = defaultDuration;
        }

        #region ITimer
        
        public string ID { get; private set; }
        
        public ETimerState State { get; private set; }

        public float Progress
        {
            get
            {
                switch (State)
                {
                    case ETimerState.INACTIVE:
                        return 0f;
                    
                    case ETimerState.STARTED:
                    case ETimerState.PAUSED:
                    case ETimerState.FINISHED:
                        if ((currentDuration - EPSILON) < 0f)
                            return 0f;
                        
                        return (timeElapsed / currentDuration).Clamp(0f, 1f);
                }

                return -1f;
            }
        }
        
        public ITimerFinishedNotifier Callback { get; set; }
        
        public bool Repeat { get; set; }
        
        public void Reset()
        {
            State = ETimerState.INACTIVE;

            timeElapsed = 0f;

            currentDuration = defaultDuration;
        }

        public void Start()
        {
            State = ETimerState.STARTED;

            timeElapsed = 0f;
        }

        public void Pause()
        {
            State = ETimerState.PAUSED;
        }

        public void Resume()
        {
            State = ETimerState.STARTED;
        }

        public void Abort()
        {
            State = ETimerState.INACTIVE;

            timeElapsed = 0f;
        }

        public void Finish()
        {
            State = ETimerState.FINISHED;
            
            
        }
        
        #endregion

        #region IRuntimeTimer
        
        public float Countdown { get; }
        public float TimeElapsed { get; }
        public float CurrentDuration { get; }
        public float DefaultDuration { get; }
        public void Reset(float duration)
        {
            throw new NotImplementedException();
        }

        public void Start(float duration)
        {
            throw new NotImplementedException();
        }
        
        #endregion

        #region ITickable

        public void Tick(float delta)
        {
            
        }

        #endregion
    }
}