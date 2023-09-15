using OcrInvoiceBackend.Application.Features.InvoiceFeatures;
using OcrInvoiceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Repositories
{
    public interface IInvoiceRepository : IBaseRepository<Invoice>
    {
        public Task<Invoice?> GetByName(string name, CancellationToken cancellationToken);
        public Task<List<Invoice>> GetByIds(List<Guid> guids, CancellationToken cancellationToken);
        public Task<bool> HasScanResults(Guid invoiceId, CancellationToken cancellationToken);
    }
}
