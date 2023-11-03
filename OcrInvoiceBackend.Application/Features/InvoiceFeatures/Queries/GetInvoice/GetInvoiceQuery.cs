using MediatR;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Queries.GetInvoice
{
    public sealed record GetInvoiceQuery(Guid Id) : IRequest<FullInvoiceDto>;
}
