using MediatR;
using OcrInvoiceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.AnalyzeInvoices
{
    public record AnalyzeInvoicesCommand(List<string> Ids) : IRequest;
}
