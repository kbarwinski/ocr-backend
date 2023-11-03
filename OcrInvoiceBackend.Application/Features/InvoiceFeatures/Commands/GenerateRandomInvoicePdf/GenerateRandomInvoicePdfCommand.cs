using MediatR;
using OcrInvoiceBackend.Application.Common.Behaviors;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.GenerateRandomInvoicePdf
{
    [RequiresRole("User")]
    public sealed record GenerateRandomInvoicePdfCommand() : IRequest<byte[]>;
}
