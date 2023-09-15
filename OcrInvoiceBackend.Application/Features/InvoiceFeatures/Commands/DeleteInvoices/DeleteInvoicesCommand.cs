using MediatR;
using OcrInvoiceBackend.Application.Common.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.DeleteInvoices
{
    [RequiresRole("User")]
    public record DeleteInvoicesCommand(List<Guid> Ids) : IRequest;
}
