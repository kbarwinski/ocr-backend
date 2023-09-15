using OcrInvoiceBackend.Application.Services.TextRecognition;
using OcrInvoiceBackend.Domain.Entities;
using OcrInvoiceBackend.Domain.Entities;
using OcrInvoiceBackend.TextRecognition.Implementations.ParsingRules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.TextRecognition.Implementations.Tesseract.ParsingFields
{
    public class SellerNIP : IParsingField
    {
        public string Name => "SellerNIP";

        public int DeductionOrder => 2;

        public void DeduceFromResults(List<Detail> results)
        {
            var targetField = results.FirstOrDefault(x => x.Name == Name);
            var buyerNip = results.FirstOrDefault(x => x.Name == "BuyerNIP");

            if (targetField?.Value == buyerNip.Value)
            {
                targetField.Value = null;
                targetField.Certainty = 0.0f;
            }
        }

        public async Task<Detail> ParseForField(ScanResults scanResults)
        {
            var isCompact = scanResults.ContentBounds.Area <= 0.7 * scanResults.ImageBounds.Area;

            var toParse = isCompact ?
                scanResults
                    .RawTextWhere(x => true) :
                scanResults
                    .RawTextWhere(x => x.Bounds.X2 <= scanResults.ContentBounds.X2 * 0.55)
                    .ToLower();

            var nipRegex = @"\b\d(?:-?\d){9}\b";

            var parsingRes = new RegexRule(nipRegex).Parse(toParse);
            if (parsingRes != null)
                return new Detail { Name = Name, Value = parsingRes.FoundText.Replace("-", ""), Certainty = 1.0f };

            return new Detail { Name = Name, Value = null, Certainty = 0.0f };
        }

        public bool ValidateDetail(Detail detail)
        {
            if (detail == null)
                return false;

            string nipRegex = @"^\d{10}$"; // Example regex for a 10-digit NIP
            bool isValidNIP = Regex.IsMatch(detail.Value, nipRegex);

            return isValidNIP;
        }
    }
}
