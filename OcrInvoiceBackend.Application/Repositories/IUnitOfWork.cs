using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Repositories
{
    public interface IUnitOfWork
    {
        public Task Save(CancellationToken cancellationToken);
    }
}
