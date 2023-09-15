using MediatR;
using Microsoft.AspNetCore.Http;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;
using OcrInvoiceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.UploadInvoices
{
    public sealed record UploadInvoicesCommand(FileType Type, List<IFormFile> Files) : IRequest
    {
    }
}
