using MediatR;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.AnalyzeInvoice
{
    public record AnalyzeInvoiceCommand(string Id) : IRequest<List<DetailDto>>;
}
