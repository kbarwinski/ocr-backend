using MediatR;
using OcrInvoiceBackend.Application.Common.Behaviors;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;
using OcrInvoiceBackend.Domain.Entities;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.UpdateInvoiceDetails
{
    [RequiresRole("User")]
    public sealed record UpdateInvoiceDetailsCommand(string Id, List<Detail> Details) : IRequest<FullInvoiceDto>
    {
    }
}
