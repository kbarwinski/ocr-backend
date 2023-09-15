using OcrInvoiceBackend.Application.Services.TextRecognition;
using OcrInvoiceBackend.Domain.Entities;
using OcrInvoiceBackend.Domain.Entities;
using OcrInvoiceBackend.TextRecognition.Implementations.ParsingRules;
using OcrInvoiceBackend.TextRecognition.Implementations.Tesseract.ParsingFields.Helpers;

namespace OcrInvoiceBackend.TextRecognition.Implementations.Tesseract.ParsingFields
{
    public class paymentDate : IParsingField
    {
        public string Name => "DeliveryDate";

        public int DeductionOrder => 6;

        public void DeduceFromResults(List<Detail> results)
        {
            var targetField = results.FirstOrDefault(x => x.Name == Name);

            var paymentType = results.FirstOrDefault(x => x.Name == "PaymentType");

            var issueDate = results.FirstOrDefault(x => x.Name == "IssueDate");
            var paymentDate = results.FirstOrDefault(x => x.Name == "PaymentDate");

            var fromPayment = issueDate.Value == null || DateParsingHelper.ToDate(issueDate.Value) == null;

            var shouldDeduce = fromPayment ?
                paymentDate.Value == null || DateParsingHelper.ToDate(paymentDate.Value) == null ? false :
                true : true;

            if (!shouldDeduce)
                return;

            if (targetField.Value == null || targetField.Value == "")
            {
                if (paymentType.Value != "bankTransfer")
                {
                    targetField.Value = fromPayment ? paymentDate.Value : issueDate.Value;
                    targetField.Certainty = 0.75f;
                }
                else if (paymentType.Value != null)
                {
                    var paymentDateTime = DateParsingHelper.ToDate(paymentDate.Value);

                    targetField.Value = fromPayment ? paymentDateTime.Value.AddDays(-14).ToString("dd.MM.yyyy") : issueDate.Value;
                    targetField.Certainty = targetField.Value == "N/A" ? 0.0f : 0.75f;
                }
            }
        }

        public async Task<Detail> ParseForField(ScanResults scanResults)
        {
            var toParse = scanResults.RawTextWhere(x => true).ToLower();

            var targetPhrases = new string[] { "data dostawy", "data dostarczenia", "data sprzedaży", "data zakończenia dostawy" };

            foreach (var phrase in targetPhrases)
            {
                var parsingRes = new HammingSimilarityRule(phrase)
                    .Parse(toParse);

                if (parsingRes != null && parsingRes.Certainty >= 0.8f)
                {
                    var reg = @"\b\d{2,4}[-./]\d{2}[-./]\d{2,4}\b";
                    var dateParsingRes = new RegexRule(reg).Parse(toParse.Substring(parsingRes.FoundIndex));

                    if (dateParsingRes?.FoundText != null)
                        return new Detail { Name = Name, Value = DateParsingHelper.ConvertToStandardFormat(dateParsingRes.FoundText), Certainty = parsingRes.Certainty * dateParsingRes.Certainty };
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

            if (string.IsNullOrEmpty(detail.Value))
            {
                return false;
            }

            var formattedDate = DateParsingHelper.ConvertToStandardFormat(detail.Value);

            if (formattedDate == null)
                return false;

            detail.Value = formattedDate;

            return true;
        }
    }
}
