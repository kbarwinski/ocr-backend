using OcrInvoiceBackend.Application.Repositories;
using OcrInvoiceBackend.Application.Services.TextRecognition;

namespace OcrInvoiceBackend.Persistence.Repositories
{
    public class ParsingFieldRepository : IParsingFieldRepository
    {
        private readonly IEnumerable<IParsingField> _parsingFields;

        public ParsingFieldRepository(IEnumerable<IParsingField> parsingFields)
        {
            _parsingFields = parsingFields;
        }

        public async Task<IEnumerable<IParsingField>> GetByCondition(Func<IParsingField, bool> predicate, CancellationToken cancellationToken)
        {
            return _parsingFields.Where(predicate)
                .OrderBy(x => x.DeductionOrder)
                .ToList();
        }
    }
}
