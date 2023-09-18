using MediatR;
using OcrInvoiceBackend.Application.Common.Behaviors;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;
using OcrInvoiceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.UpdateInvoice
{
    [RequiresRole("User")]
    public sealed record UpdateInvoiceCommand(string Id, InvoiceUpdateModel ToUpdate) : IRequest<FullInvoiceDto>
    {
    }
}
