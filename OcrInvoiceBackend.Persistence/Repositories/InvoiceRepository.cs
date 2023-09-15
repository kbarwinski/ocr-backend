using Microsoft.EntityFrameworkCore;
using OcrInvoiceBackend.Application.Repositories;
using OcrInvoiceBackend.Domain.Entities;
using OcrInvoiceBackend.Persistence.Context;

namespace OcrInvoiceBackend.Persistence.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(DataContext context) : base(context)
        {
        }

        public Task<List<Invoice>> GetByIds(List<Guid> guids, CancellationToken cancellationToken)
        {
            return Context.Invoices
                .Where(x => guids.Contains(x.Id))
                .Include(x => x.Scan)
                .Include(x => x.Details)
                .ToListAsync(cancellationToken);
        }

        public Task<Invoice?> GetByName(string name, CancellationToken cancellationToken)
        {
            return Get(x => x.Name == name, cancellationToken);
        }

        public async Task<bool> HasScanResults(Guid invoiceId, CancellationToken cancellationToken)
        {
            return await Context.Invoices
                .AnyAsync(i => i.Id == invoiceId && i.Scan != null);
        }
    }
}
