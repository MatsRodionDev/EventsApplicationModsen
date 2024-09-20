namespace EventsApplication.Application.Common.Interfaces.Queues
{
    public interface IEventUpdateQueue
    {
        Task QueueAsync(Guid userId);
        Task<Guid> DequeueAsync(CancellationToken cancellationToken);
    }
}
