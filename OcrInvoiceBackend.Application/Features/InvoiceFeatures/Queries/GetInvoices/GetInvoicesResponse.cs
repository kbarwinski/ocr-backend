using OcrInvoiceBackend.Application.Common.Models;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Queries.GetInvoices
{
    public sealed record GetInvoicesResponse()
    {
        public List<InvoiceDto> Result { get; set; }
        public PaginationResponse Pagination { get; set; }
    }
}
