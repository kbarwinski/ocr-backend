using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using OcrInvoiceBackend.Application.Services.BackgroundQueue;
using OcrInvoiceBackend.Infrastructure.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Infrastructure.Services.BackgroundQueue
{
    public class BackgroundQueueService : BackgroundService, IBackgroundQueueService
    {
        private IBackgroundQueue _taskQueue;
        private IHubContext<ProgressHub, IProgressClient> _hubContext;

        public BackgroundQueueService(IBackgroundQueue taskQueue, IHubContext<ProgressHub, IProgressClient> hubContext)
        {
            _taskQueue = taskQueue;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var workItem = await _taskQueue.DequeueAsync(cancellationToken);
                if (workItem == null)
                    continue;

                try
                {
                    var res = await workItem(cancellationToken);

                    await _hubContext.Clients.All.ReceiveMessage(JsonConvert.SerializeObject(res));
                }
                catch (Exception ex)
                {
                }
            }
        }

        async Task IBackgroundQueueService.ExecuteAsync(CancellationToken cancellationToken)
        {
            await ExecuteAsync(cancellationToken);
        }
    }
}
