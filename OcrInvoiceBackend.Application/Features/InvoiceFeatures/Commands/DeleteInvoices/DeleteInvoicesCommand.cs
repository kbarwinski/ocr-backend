using MediatR;
using OcrInvoiceBackend.Application.Common.Behaviors;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.DeleteInvoices
{
    [RequiresRole("Admin")]
    public record DeleteInvoicesCommand(List<Guid> Ids) : IRequest;
}
