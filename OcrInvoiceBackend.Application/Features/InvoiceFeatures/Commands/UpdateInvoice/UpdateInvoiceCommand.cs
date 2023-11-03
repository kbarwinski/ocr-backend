using MediatR;
using OcrInvoiceBackend.Application.Common.Behaviors;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.UpdateInvoice
{
    [RequiresRole("User")]
    public sealed record UpdateInvoiceCommand(string Id, InvoiceUpdateModel ToUpdate) : IRequest<FullInvoiceDto>
    {
    }
}
