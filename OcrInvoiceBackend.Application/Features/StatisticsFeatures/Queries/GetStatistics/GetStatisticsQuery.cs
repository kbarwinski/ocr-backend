using MediatR;
using OcrInvoiceBackend.Application.Common.Models;

namespace OcrInvoiceBackend.Application.Features.StatisticsFeatures.Queries.GetStatistics
{
    public sealed record GetStatisticsQuery(int Page, int PageSize, string SortingOrders, DateTime? Start, DateTime? End) :
    PaginationQuery(Page, PageSize, SortingOrders), IRequest<GetStatisticsResponse>;
}
