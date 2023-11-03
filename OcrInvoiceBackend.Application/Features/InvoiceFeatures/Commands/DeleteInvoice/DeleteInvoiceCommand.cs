using MediatR;
using OcrInvoiceBackend.Application.Common.Behaviors;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.DeleteInvoice
{
    [RequiresRole("Admin")]
    public sealed record DeleteInvoiceCommand(Guid Id) : IRequest;
}
