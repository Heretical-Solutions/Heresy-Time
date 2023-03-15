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
            
            context.SetState(ETimerState.INACTIVE);
        }

        public void Start(IRuntimeTimerContext context)
        {
            context.TimeElapsed = 0f;
            
            context.SetState(ETimerState.STARTED);
        }

        public void Pause(IRuntimeTimerContext context)
        {
            context.SetState(ETimerState.PAUSED);
        }

        public void Resume(IRuntimeTimerContext context)
        {
            context.SetState(ETimerState.STARTED);
        }
        
        public void Abort(IRuntimeTimerContext context)
        {
            context.SetState(ETimerState.INACTIVE);

            context.TimeElapsed = 0f;
        }
        
        public void Finish(IRuntimeTimerContext context)
        {
            context.SetState(ETimerState.FINISHED);
            
            //TODO: CALLBACK!!!!
        }

        public void Tick(IRuntimeTimerContext context, float delta)
        {
        }
    }
}