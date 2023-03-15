namespace HereticalSolutions.Time.Strategies
{
    public class RuntimeFinishedStrategy : ITimerStrategy<IRuntimeTimerContext>
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
            context.TimeElapsed += delta;
            
            if (context.TimeElapsed > context.CurrentDuration)
                Finish(context);
        }
    }
}