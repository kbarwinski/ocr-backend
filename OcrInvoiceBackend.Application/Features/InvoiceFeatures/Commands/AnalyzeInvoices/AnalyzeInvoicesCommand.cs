using MediatR;
using OcrInvoiceBackend.Application.Common.Behaviors;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.AnalyzeInvoices
{
    [RequiresRole("User")]
    public record AnalyzeInvoicesCommand(List<string> Ids) : IRequest;
}
