using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.ScanInvoices
{
    public record ScanInvoicesCommand(List<string> InvoiceIds) : IRequest;
}
