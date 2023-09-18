using MediatR;
using OcrInvoiceBackend.Application.Common.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.ScanInvoices
{
    [RequiresRole("User")]
    public record ScanInvoicesCommand(List<string> InvoiceIds) : IRequest;
}
