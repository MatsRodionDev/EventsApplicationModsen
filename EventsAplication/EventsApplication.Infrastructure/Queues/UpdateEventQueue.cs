using EventsApplication.Application.Common.Interfaces.Queues;
using System.Collections.Concurrent;

namespace EventsApplication.Infrastructure.Queues
{
    public class UpdateEventQueue : IEventUpdateQueue
    {
        private readonly ConcurrentQueue<Guid> _queue = new ConcurrentQueue<Guid>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public Task QueueAsync(Guid item)
        {
            _queue.Enqueue(item);
            _signal.Release();
            return Task.CompletedTask;
        }

        public async Task<Guid> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _queue.TryDequeue(out var item);
            return item;
        }
    }
}
