using HereticalSolutions.Delegates;

using HereticalSolutions.Repositories;

namespace HereticalSolutions.Time.Timers
{
    public class RuntimeTimer
        : ITimer,
          IRuntimeTimer,
          IRuntimeTimerContext,
          ITimerWithState,
          ITickable
    {
        private ITimerStrategy<IRuntimeTimerContext> currentStrategy;

        private readonly IReadOnlyRepository<ETimerState, ITimerStrategy<IRuntimeTimerContext>> strategyRepository;

        public RuntimeTimer(
            string id,
            float defaultDuration,
            
            IPublisherSingleArgGeneric<ITimer> onStartAsPublisher,
            INonAllocSubscribableSingleArgGeneric<ITimer> onStartAsSubscribable,
            
            IPublisherSingleArgGeneric<ITimer> onFinishAsPublisher,
            INonAllocSubscribableSingleArgGeneric<ITimer> onFinishAsSubscribable,
            
            IReadOnlyRepository<ETimerState, ITimerStrategy<IRuntimeTimerContext>> strategyRepository)
        {
            ID = id;

            TimeElapsed = 0f;

            CurrentDuration = DefaultDuration = defaultDuration;
            

            OnStartAsPublisher = onStartAsPublisher;
            
            OnStart = onStartAsSubscribable;
            
            
            OnFinishAsPublisher = onFinishAsPublisher;

            OnFinish = onFinishAsSubscribable;
            
            
            this.strategyRepository = strategyRepository;
            
            SetState(ETimerState.INACTIVE);
        }

        #region IRuntimeTimerContext
        
        public float TimeElapsed { get; set; }

        public float CurrentDuration { get; set; }

        public float DefaultDuration { get; private set; }

        public IPublisherSingleArgGeneric<ITimer> OnStartAsPublisher { get; private set; }
        
        public IPublisherSingleArgGeneric<ITimer> OnFinishAsPublisher { get; private set; }

        #endregion
        
        #region ITimer
        
        public string ID { get; private set; }
        
        public ETimerState State { get; private set; }

        public float Progress
        {
            get => currentStrategy.GetProgress(this);
        }

        public INonAllocSubscribableSingleArgGeneric<ITimer> OnStart { get; private set; }
        
        public INonAllocSubscribableSingleArgGeneric<ITimer> OnFinish { get; private set; }

        public bool Repeat { get; set; }
        
        public void Reset()
        {
            currentStrategy.Reset(this);
        }

        public void Start()
        {
            currentStrategy.Start(this);
        }

        public void Pause()
        {
            currentStrategy.Pause(this);
        }

        public void Resume()
        {
            currentStrategy.Resume(this);
        }

        public void Abort()
        {
            currentStrategy.Abort(this);
        }

        public void Finish()
        {
            currentStrategy.Finish(this);
        }
        
        #endregion

        #region IRuntimeTimer
        
        public float Countdown
        {
            get => (CurrentDuration - TimeElapsed);
        }
        
        public void Reset(float duration)
        {
            Reset();

            CurrentDuration = duration;
        }

        public void Start(float duration)
        {
            Start();

            CurrentDuration = duration;
        }
        
        #endregion

        #region ITickable

        public void Tick(float delta)
        {
            currentStrategy.Tick(this, delta);
        }

        #endregion

        #region ITimerWithState

        public void SetState(ETimerState state)
        {
            State = state;
            
            currentStrategy = strategyRepository.Get(state);
        }

        #endregion
    }
}