using MediatR;
using OcrInvoiceBackend.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Queries.GetInvoices
{
    public sealed record GetInvoicesQuery(int Page, int PageSize, string SortingOrders, DateTime? Start, DateTime? End, bool? IsScanned, bool? IsParsed, bool? IsApproved) :
        PaginationQuery(Page, PageSize, SortingOrders), IRequest<GetInvoicesResponse>;
}

