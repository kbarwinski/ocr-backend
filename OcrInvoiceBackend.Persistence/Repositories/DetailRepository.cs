using OcrInvoiceBackend.Application.Repositories;
using OcrInvoiceBackend.Domain.Entities;
using OcrInvoiceBackend.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Persistence.Repositories
{
    public class DetailRepository : BaseRepository<Detail>, IDetailRepository
    {
        public DetailRepository(DataContext context) : base(context)
        {
        }
    }
}
