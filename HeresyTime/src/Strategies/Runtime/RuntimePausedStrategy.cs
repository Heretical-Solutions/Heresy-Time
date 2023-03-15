namespace HereticalSolutions.Time.Strategies
{
    public class RuntimePausedStrategy : ITimerStrategy<IRuntimeTimerContext>
    {
        public float GetProgress(IRuntimeTimerContext context)
        {
            if ((context.CurrentDuration - MathHelpers.EPSILON) < 0f)
                return 0f;
                        
            return (context.TimeElapsed / context.CurrentDuration).Clamp(0f, 1f);
        }

        public void Reset(IRuntimeTimerContext context)
        {
            context.TimeElapsed = 0f;

            context.CurrentDuration = context.DefaultDuration;
            
            context.SetState(ETimerState.INACTIVE);
        }

        public void Start(IRuntimeTimerContext context)
        {
            //ENSURE THAT CALLING START() ON A PAUSED TIMER SHOULD IGNORE A CALL INSTEAD OF RESETTING THE TIMER
        }

        public void Pause(IRuntimeTimerContext context)
        {
            //Why bother?
        }

        public void Resume(IRuntimeTimerContext context)
        {
            context.SetState(ETimerState.STARTED);
        }
        
        public void Abort(IRuntimeTimerContext context)
        {
            context.TimeElapsed = 0f;
            
            context.SetState(ETimerState.INACTIVE);
        }
        
        public void Finish(IRuntimeTimerContext context)
        {
            context.SetState(ETimerState.FINISHED);
            
            context.OnFinishAsPublisher.Publish((ITimer)context);
        }

        public void Tick(IRuntimeTimerContext context, float delta)
        {
            //Why bother?
        }
    }
}