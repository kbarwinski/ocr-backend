using MediatR;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Queries.GetInvoice
{
    public sealed record GetInvoiceQuery(Guid Id) : IRequest<FullInvoiceDto>;
}
