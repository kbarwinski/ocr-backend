using MediatR;
using OcrInvoiceBackend.Application.Common.Behaviors;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.ScanInvoices
{
    [RequiresRole("User")]
    public record ScanInvoicesCommand(List<string> InvoiceIds) : IRequest;
}
