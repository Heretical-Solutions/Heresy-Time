namespace HereticalSolutions.Time
{
	public interface ITimerFinishedNotifier
	{
		void NotifyTimerFinished(ITimer timer);
	}
}