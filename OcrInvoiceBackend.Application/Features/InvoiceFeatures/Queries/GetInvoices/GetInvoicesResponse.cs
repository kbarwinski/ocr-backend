using OcrInvoiceBackend.Application.Common.Models;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Queries.GetInvoices
{
    public sealed record GetInvoicesResponse()
    {
        public List<InvoiceDto> Result { get; set; }
        public PaginationResponse Pagination { get; set; }
    }
}
