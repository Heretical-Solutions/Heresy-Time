namespace HereticalSolutions.Time.Strategies
{
    public class RuntimeInactiveStrategy : ITimerStrategy<IRuntimeTimerContext>
    {
        public float GetProgress(IRuntimeTimerContext context)
        {
            return 0f;
        }

        public void Reset(IRuntimeTimerContext context)
        {
            context.TimeElapsed = 0f;

            context.CurrentDuration = context.DefaultDuration;
        }

        public void Start(IRuntimeTimerContext context)
        {
            context.TimeElapsed = 0f;
            
            context.SetState(ETimerState.STARTED);
            
            context.OnStartAsPublisher.Publish((ITimer)context);
        }

        public void Pause(IRuntimeTimerContext context)
        {
            //Why bother?
        }

        public void Resume(IRuntimeTimerContext context)
        {
            //Why bother?
        }
        
        public void Abort(IRuntimeTimerContext context)
        {
            context.TimeElapsed = 0f;
        }
        
        public void Finish(IRuntimeTimerContext context)
        {
            //ENSURE WHETHER CALLING FINISH() ON INACTIVE TIMER SHOULD NOT BE CALLING A CALLBACK
        }

        public void Tick(IRuntimeTimerContext context, float delta)
        {
            //Why bother?
        }
    }
}