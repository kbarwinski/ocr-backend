using OcrInvoiceBackend.Application.Repositories;
using OcrInvoiceBackend.Domain.Entities;
using OcrInvoiceBackend.Persistence.Context;


namespace OcrInvoiceBackend.Persistence.Repositories
{
    public class ScanResultsRepository : BaseRepository<ScanResults>, IScanResultsRepository
    {
        public ScanResultsRepository(DataContext context) : base(context)
        {
        }
    }
}
