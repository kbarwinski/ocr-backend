using OcrInvoiceBackend.Domain.Entities;

namespace OcrInvoiceBackend.Application.Repositories
{
    public interface IStatisticsRepository : IBaseRepository<Statistics>
    {
        Task<Statistics> GetTodayStatistics();
    }
}
