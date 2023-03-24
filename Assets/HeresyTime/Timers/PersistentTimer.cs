using System;
using HereticalSolutions.Delegates;
using HereticalSolutions.Persistence;
using HereticalSolutions.Repositories;

namespace HereticalSolutions.Time.Timers
{
    public class PersistentTimer
        : ITimer,
          IPersistentTimer,
          IPersistentTimerContext,
          ITimerWithState,
          ITickable,
          IVisitable
    {
        private ITimerStrategy<IPersistentTimerContext, TimeSpan> currentStrategy;

        private readonly IReadOnlyRepository<ETimerState, ITimerStrategy<IPersistentTimerContext, TimeSpan>> strategyRepository;

        public PersistentTimer(
            string id,
            TimeSpan defaultDurationSpan,
            
            IPublisherSingleArgGeneric<IPersistentTimer> onStartAsPublisher,
            INonAllocSubscribableSingleArgGeneric<IPersistentTimer> onStartAsSubscribable,
            
            IPublisherSingleArgGeneric<IPersistentTimer> onFinishAsPublisher,
            INonAllocSubscribableSingleArgGeneric<IPersistentTimer> onFinishAsSubscribable,
            
            IReadOnlyRepository<ETimerState, ITimerStrategy<IPersistentTimerContext, TimeSpan>> strategyRepository)
        {
            ID = id;

            StartTime = default(DateTime);
            
            EstimatedFinishTime = default(DateTime);

            SavedProgress = default(TimeSpan);

            CurrentDurationSpan = DefaultDurationSpan = defaultDurationSpan;
            

            OnStartAsPublisher = onStartAsPublisher;
            
            OnStart = onStartAsSubscribable;
            
            
            OnFinishAsPublisher = onFinishAsPublisher;

            OnFinish = onFinishAsSubscribable;
            
            
            this.strategyRepository = strategyRepository;
            
            SetState(ETimerState.INACTIVE);
        }

        #region IPersistentTimerContext
        
        #region Variables
        
        public DateTime StartTime { get; set; }
        
        public DateTime EstimatedFinishTime { get; set; }

        public TimeSpan SavedProgress { get; set; }
        
        #endregion

        #region Duration
        
        public TimeSpan CurrentDurationSpan { get; set; }
        
        public TimeSpan DefaultDurationSpan { get; set; }
        
        #endregion

        #region Publishers
        
        public IPublisherSingleArgGeneric<IPersistentTimer> OnStartAsPublisher { get; private set; }
        
        public IPublisherSingleArgGeneric<IPersistentTimer> OnFinishAsPublisher { get; private set; }

        #endregion
        
        #endregion
        
        #region ITimer
        
        public string ID { get; private set; }
        
        #region Timer state
        
        public ETimerState State { get; private set; }

        #endregion

        #region Progress
        
        public float Progress
        {
            get => currentStrategy.GetProgress(this);
        }

        #endregion

        #region Controls

        public bool Accumulate { get; set; }

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
        
        #endregion

        #region IPersistentTimer

        #region Countdown and Time elapsed

        public TimeSpan TimeElapsedSpan
        {
            get => currentStrategy.GetTimeElapsed(this);
        }

        public TimeSpan CountdownSpan
        {
            get => currentStrategy.GetCountdown(this);
        }

        #endregion
        
        #region Controls
        
        public void Reset(TimeSpan durationSpan)
        {
            Reset();
            
            CurrentDurationSpan = durationSpan;
        }

        public void Start(TimeSpan durationSpan)
        {
            CurrentDurationSpan = durationSpan;
            
            Start();
        }
        
        #endregion

        #region Callbacks

        public INonAllocSubscribableSingleArgGeneric<IPersistentTimer> OnStart { get; private set; }
        
        public INonAllocSubscribableSingleArgGeneric<IPersistentTimer> OnFinish { get; private set; }

        #endregion
        
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

        #region IVisitable
        
        public void Accept(ISaveVisitor visitor)
        {
            //visitor.Save<IPersistentTimer, PersistentTimerDTO>(this, out PersistentTimerDTO dto);
        }

        public void Accept(ILoadVisitor visitor)
        {
            //visitor.Load<IPersistentTimer, PersistentTimerDTO>(PersistentTimerDTO dto);
        }
        
        #endregion
    }
}