using OcrInvoiceBackend.Application.Services.BackgroundQueue;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Infrastructure.Services.BackgroundQueue
{
    public class BackgroundQueue : IBackgroundQueue
    {
        private ConcurrentQueue<Func<CancellationToken, Task<object>>> _workItems =
            new ConcurrentQueue<Func<CancellationToken, Task<object>>>();
        private SemaphoreSlim _signal = new SemaphoreSlim(15);

        public async Task EnqueueAsync(Func<CancellationToken, Task<object>> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            _workItems.Enqueue(workItem);
            _signal.Release();
        }

        public async Task<Func<CancellationToken, Task<object>>> DequeueAsync(
            CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);

            return workItem;
        }
    }
}
