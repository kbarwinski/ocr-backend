using MediatR;
using OcrInvoiceBackend.Application.Common.Behaviors;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.AnalyzeInvoice
{
    [RequiresRole("User")]
    public record AnalyzeInvoiceCommand(string Id) : IRequest<List<DetailDto>>;
}
