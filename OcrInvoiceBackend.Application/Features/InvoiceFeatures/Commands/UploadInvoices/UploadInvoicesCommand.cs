using MediatR;
using Microsoft.AspNetCore.Http;
using OcrInvoiceBackend.Application.Common.Behaviors;
using OcrInvoiceBackend.Domain.Entities;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.UploadInvoices
{
    [RequiresRole("User")]
    public sealed record UploadInvoicesCommand(FileType Type, List<IFormFile> Files) : IRequest
    {
    }
}
