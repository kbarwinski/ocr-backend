using MediatR;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.ScanInvoice
{
    public sealed record ScanInvoiceCommand(string InvoiceId) : IRequest<FullInvoiceDto>;
}
