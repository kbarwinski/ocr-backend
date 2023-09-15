using MediatR;
using OcrInvoiceBackend.Application.Common.Behaviors;
using OcrInvoiceBackend.Application.Common.Models;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Queries.GetInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.StatisticsFeatures.Queries.GetStatistics
{
    [RequiresRole("Admin")]
    public sealed record GetStatisticsQuery(int Page, int PageSize, string SortingOrders, DateTime? Start, DateTime? End) :
    PaginationQuery(Page, PageSize, SortingOrders), IRequest<GetStatisticsResponse>;
}
