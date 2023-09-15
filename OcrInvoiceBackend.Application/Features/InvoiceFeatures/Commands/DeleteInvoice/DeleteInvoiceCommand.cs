using MediatR;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.DeleteInvoice
{
    public sealed record DeleteInvoiceCommand(Guid Id) : IRequest;
}
