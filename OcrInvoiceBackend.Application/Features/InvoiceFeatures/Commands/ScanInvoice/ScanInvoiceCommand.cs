using MediatR;
using OcrInvoiceBackend.Application.Common.Behaviors;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.ScanInvoice
{
    [RequiresRole("User")]
    public sealed record ScanInvoiceCommand(string InvoiceId) : IRequest<FullInvoiceDto>;
}
