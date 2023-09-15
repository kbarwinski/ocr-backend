using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Services.BackgroundQueue
{
    public interface IBackgroundQueue
    {
#pragma warning disable CS4014 // To wywołanie nie jest oczekiwane, dlatego wykonywanie bieżącej metody będzie kontynuowane do czasu ukończenia wywołania
        Task EnqueueAsync(Func<CancellationToken, Task<object>> workItem);
#pragma warning restore CS4014 // To wywołanie nie jest oczekiwane, dlatego wykonywanie bieżącej metody będzie kontynuowane do czasu ukończenia wywołania

        Task<Func<CancellationToken, Task<object>>> DequeueAsync(CancellationToken cancellationToken);
    }

}
