using OcrInvoiceBackend.Application.Services.TextRecognition;
using OcrInvoiceBackend.Domain.Entities;
using OcrInvoiceBackend.Domain.Entities;
using OcrInvoiceBackend.TextRecognition.Implementations.ParsingRules;
using OcrInvoiceBackend.TextRecognition.Implementations.Tesseract.ParsingFields.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.TextRecognition.Implementations.Tesseract.ParsingFields
{
    public class PaymentDate : IParsingField
    {
        public string Name => "PaymentDate";

        public int DeductionOrder => 7;

        public void DeduceFromResults(List<Detail> results)
        {
            var targetField = results.FirstOrDefault(x => x.Name == Name);

            var paymentType = results.FirstOrDefault(x => x.Name == "PaymentType");

            var issueDate = results.FirstOrDefault(x => x.Name == "IssueDate");
            var deliveryDate = results.FirstOrDefault(x => x.Name == "DeliveryDate");

            var fromDelivery = issueDate.Value == null || DateParsingHelper.ToDate(issueDate.Value) == null;

            var shouldDeduce = fromDelivery ?
                deliveryDate.Value == null || DateParsingHelper.ToDate(deliveryDate.Value) == null ? false :
                true : true;

            if (!shouldDeduce)
                return;

            var targetDateTime = fromDelivery ? DateParsingHelper.ToDate(deliveryDate.Value) :
                DateParsingHelper.ToDate(issueDate.Value);

            if (targetField.Value == null || targetField.Value == "")
            {
                if (paymentType.Value != "bankTransfer")
                {
                    targetField.Value = fromDelivery ? deliveryDate.Value : issueDate.Value;
                    targetField.Certainty = 0.75f;
                }
                else if (paymentType.Value != null)
                {
                    targetField.Value = targetDateTime.Value.AddDays(14).ToString("dd.MM.yyyy");

                    targetField.Certainty = 0.75f;
                }
            }
        }

        public async Task<Detail> ParseForField(ScanResults scanResults)
        {
            var toParse = scanResults.RawTextWhere(x => true).ToLower();

            var targetPhrases = new string[] { "termin zapłaty", "termin płatności", "płatność do", "do zapłaty", "terminie", "zapłaty", };

            foreach (var phrase in targetPhrases)
            {
                var parsingRes = new HammingSimilarityRule(phrase)
                    .Parse(toParse);

                if (parsingRes != null && parsingRes.Certainty >= 0.8f)
                {
                    var reg = @"\b\d{2,4}[-./]\d{2}[-./]\d{2,4}\b";

                    var dateParsingRes = new RegexRule(reg).Parse(toParse.Substring(parsingRes.FoundIndex));
                    var dateTime = DateParsingHelper.ConvertToStandardFormat(dateParsingRes?.FoundText);

                    if (dateTime != null)
                        return new Detail { Name = Name, Value = dateTime, Certainty = parsingRes.Certainty * dateParsingRes.Certainty };
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
