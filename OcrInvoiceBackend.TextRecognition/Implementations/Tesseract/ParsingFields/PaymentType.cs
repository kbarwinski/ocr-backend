using OcrInvoiceBackend.Application.Services.TextRecognition;
using OcrInvoiceBackend.Domain.Entities;
using OcrInvoiceBackend.Domain.Entities;
using OcrInvoiceBackend.TextRecognition.Implementations.ParsingRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.TextRecognition.Implementations.Tesseract.ParsingFields
{
    public class PaymentType : IParsingField
    {
        public string Name => "PaymentType";

        public int DeductionOrder => 4;

        private readonly Dictionary<string, string> paymentTypes = new Dictionary<string, string>
        {
            { "gotówka", "cash" },
            { "kartą", "card" },
            { "przelewy24", "card" },
            { "payu", "card" },
            { "przelew", "bankTransfer" },
            { "pobranie", "onDelivery" },
            { "przedpłata", "upfront" }
        };

        public async Task<Detail> ParseForField(ScanResults scanResults)
        {
            var toParse = scanResults.RawTextWhere(x => true).ToLower();

            Detail highestCertaintyDetail = new Detail { Name = Name, Value = null, Certainty = 0.0f };

            foreach (var word in paymentTypes.Keys)
            {
                var parsingRes = new HammingSimilarityRule(word).Parse(toParse);

                if (parsingRes.Certainty >= 0.8f && parsingRes.Certainty > highestCertaintyDetail.Certainty)
                {
                    highestCertaintyDetail.Value = paymentTypes[word];
                    highestCertaintyDetail.Certainty = parsingRes.Certainty;
                }
            }

            return highestCertaintyDetail;
        }

        public void DeduceFromResults(List<Detail> results)
        {
        }

        public bool ValidateDetail(Detail detail)
        {
            if (detail == null)
            {
                return false;
            }

            List<string> validPaymentTypes = paymentTypes.Values.Distinct().ToList();

            if (!validPaymentTypes.Contains(detail.Value))
            {
                return false;
            }

            return true;
        }
    }
}
