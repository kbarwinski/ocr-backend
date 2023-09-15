using OcrInvoiceBackend.Application.Services.TextRecognition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Repositories
{
    public interface IParsingFieldRepository
    {
        Task<IEnumerable<IParsingField>> GetByCondition(Func<IParsingField, bool> predicate, CancellationToken cancellationToken);
    }
}
