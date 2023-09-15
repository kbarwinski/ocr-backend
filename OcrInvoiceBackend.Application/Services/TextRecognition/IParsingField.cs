using OcrInvoiceBackend.Domain.Entities;

namespace OcrInvoiceBackend.Application.Services.TextRecognition
{
    public interface IParsingField
    {
        string Name { get; }

        bool ValidateDetail(Detail detail);

        Task<Detail?> ParseForField(ScanResults scanResults);

        int DeductionOrder { get; }
        void DeduceFromResults(List<Detail> results);
    }
}
