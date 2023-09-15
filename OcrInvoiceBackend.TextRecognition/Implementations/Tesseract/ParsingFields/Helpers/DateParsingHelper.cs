using System.Globalization;

namespace OcrInvoiceBackend.TextRecognition.Implementations.Tesseract.ParsingFields.Helpers
{
    public static class DateParsingHelper
    {
        public static DateTime? ToDate(string date)
        {
            string[] formats = {"dd.MM.yyyy", "dd/MM/yyyy", "dd-MM-yyyy", "yyyy-MM-dd", "yyyy/MM/dd", "yyyy.MM.dd"};

            foreach (string format in formats)
            {
                if (DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    return parsedDate;
                }
            }

            return null;
        }

        public static string ConvertToStandardFormat(string date)
        {
            DateTime? parsedDate = ToDate(date);
            return parsedDate?.ToString("dd.MM.yyyy");
        }
    }
}
