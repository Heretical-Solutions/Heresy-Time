namespace HereticalSolutions.Time.Strategies
{
    public class RuntimeFinishedStrategy : ITimerStrategy<IRuntimeTimerContext>
    {
        public float GetProgress(IRuntimeTimerContext context)
        {
            //THIS ONE IS AS EXPECTED. IF THE TIMER WAS FINISHED PREMATURELY BY A FINISH() CALL RATHER THAN TIMER ACTUALLY RUNNING OUT WE MIGHT BE CURIOUS HOW MUCH OF A PROGRESS WAS MADE SO FAR
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
            
            context.SetState(ETimerState.INACTIVE);
        }
        
        public void Finish(IRuntimeTimerContext context)
        {
            //Why bother?
        }

        public void Tick(IRuntimeTimerContext context, float delta)
        {
            //Why bother?
        }
    }
}