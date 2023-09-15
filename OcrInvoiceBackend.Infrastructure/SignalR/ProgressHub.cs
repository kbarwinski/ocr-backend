using Microsoft.AspNetCore.SignalR;
using OcrInvoiceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Infrastructure.SignalR
{
    public class ProgressHub : Hub<IProgressClient>
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.ReceiveMessage(message);
        }
    }

}
