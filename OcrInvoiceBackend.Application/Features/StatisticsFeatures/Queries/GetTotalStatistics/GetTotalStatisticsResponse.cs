using OcrInvoiceBackend.Application.Features.StatisticsFeatures.Models;

namespace OcrInvoiceBackend.Application.Features.StatisticsFeatures.Queries.GetTotalStatistics
{
    public class GetTotalStatisticsResponse
    {
        public DateTimeOffset StartDate{ get; set; }
        public DateTimeOffset EndDate { get; set; }
        public StatisticsDto Result { get; set; }
    }
}
