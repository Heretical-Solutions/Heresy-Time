using HereticalSolutions.Delegates.Factories;

using HereticalSolutions.Repositories;
using HereticalSolutions.Repositories.Factories;

using HereticalSolutions.Time.Strategies;
using HereticalSolutions.Time.Timers;

namespace HereticalSolutions.Time.Factories
{
    public static class TimersFactory
    {
        public static RuntimeTimer BuildRuntimeTimer(
            string id,
            float defaultDuration)
        {
            var onStart = DelegatesFactory.BuildNonAllocBroadcasterGeneric<ITimer>();
            
            var onFinish = DelegatesFactory.BuildNonAllocBroadcasterGeneric<ITimer>();
            
            return new RuntimeTimer(
                id,
                defaultDuration,
                
                onStart,
                onStart,
                
                onFinish,
                onFinish,
                
                BuildRuntimeStrategyRepository());
        }

        private static IReadOnlyRepository<ETimerState, ITimerStrategy<IRuntimeTimerContext>>
            BuildRuntimeStrategyRepository()
        {
            var repository = RepositoriesFactory.BuildDictionaryRepository<ETimerState, ITimerStrategy<IRuntimeTimerContext>>(
                new ETimerStateComparer());
            
            repository.Add(ETimerState.INACTIVE, new RuntimeInactiveStrategy());
            
            repository.Add(ETimerState.STARTED, new RuntimeStartedStrategy());
            
            repository.Add(ETimerState.PAUSED, new RuntimePausedStrategy());
            
            repository.Add(ETimerState.FINISHED, new RuntimeFinishedStrategy());

            return repository;
        }
    }
}