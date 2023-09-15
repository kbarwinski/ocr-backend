using OcrInvoiceBackend.Application.Services.TextRecognition;
using OcrInvoiceBackend.Domain.Entities;
using OcrInvoiceBackend.Domain.Entities;
using OcrInvoiceBackend.TextRecognition.Implementations.ParsingRules;

namespace OcrInvoiceBackend.TextRecognition.Implementations.Tesseract.ParsingFields
{
    public class DocumentType : IParsingField
    {
        public string Name => "DocumentType";

        public int DeductionOrder => 3;

        public void DeduceFromResults(List<Detail> results)
        {
        }

        public async Task<Detail> ParseForField(ScanResults scanResults)
        {
            var toParse = scanResults.RawTextWhere(x => true).ToLower();

            var documentTypeRules = new Dictionary<string, string>
            {
            { "paragon fiskalny", "receipt"},
            { "faktura vat", "invoice" },
            { "paragon", "receipt" },
            { "faktura", "invoice" }
            };

            foreach (var rule in documentTypeRules)
            {
                var parsingRes = new RegexRule(rule.Key).Parse(toParse);
                if (parsingRes != null)
                {
                    return new Detail { Name = Name, Value = rule.Value, Certainty = 1.0f };
                }
            }

            return new Detail { Name = Name, Value = null, Certainty = 0.0f };
        }

        public bool ValidateDetail(Detail detail)
        {
            if (detail == null)
            {
                return false;
            }

            List<string> validDocumentTypes = new List<string> { "receipt", "invoice" };

            if (!validDocumentTypes.Contains(detail.Value.ToLower()))
            {
                return false;
            }

            return true;
        }
    }
}
