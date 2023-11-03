using OcrInvoiceBackend.Application.Services.Automation;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OcrInvoiceBackend.Automation.Implementations.Automation
{
    public class InvoiceGeneratorService : IInvoiceGeneratorService
    {
        private class GeneratorTemplate
        {
            public string StartTemplate { get; set; }
            public string ItemTemplate { get; set; }
            public string FooterTemplate { get; set; }
            public Dictionary<string, PlaceholderValueType> StartPlaceholders { get; set; }
            public Dictionary<string, PlaceholderValueType> ItemPlaceholders { get; set; }
            public Dictionary<string, PlaceholderValueType> FooterPlaceholders { get; set; }
        }
        private enum PlaceholderValueType
        {
            FontFamily,
            Padding,
            TableWidth,
            TableBorder,
            TablePadding,

            InvoiceNumber,
            NameAndSurname,
            FullAddress,
            NIP,

            ItemDescription,
            ItemQuantity,
            ItemPrice,
            NetValue,
            VatPercent,
            GrossValue,
            Currency,

            TotalNetValue,
            TotalGrossValue,

            Date,
            DeliveryDate,
            PaymentDate,

            PaymentMethod,
            BankNumber,
            Notes
        };
        private class RandomInvoicePlaceholderValueGenerator
        {
            private readonly Random _random;

            public Dictionary<PlaceholderValueType, Func<string>> GeneratedValues;

            private string[] randomBasicFontCss = { "Arial", "Verdana", "Times New Roman", "Tahoma" };
            private string[] randomBorderCss = { "1px solid black", "2px dotted grey", "1px dashed black" };

            private string[] randomInvoicePrefix = { "INV", "FACT", "BILL" };

            private string[] randomName = { "Jan", "Anna", "Piotr", "Katarzyna" };
            private string[] randomSurname = { "Nowak", "Kowalski", "Wójcik", "Kowalczyk" };
            private string[] randomStreet = { "Warszawska", "Krakowska", "Lwowska", "Mazowiecka" };
            private string[] randomCity = { "Warszawa", "Kraków", "Wrocław", "Poznań" };

            private string[] randomPaymentMethod = { "Przelew", "Gotówka", "Karta" };
            private string[] randomBankNumber = { "PL61 1090 1014 0000 0712 1981 2874", "PL91 1240 1037 1111 0010 3946 7890", "PL50 1234 5678 1234 5678 1234 5678" };
            private string[] randomNotes = { "Zapłacono z góry", "Oczekiwanie na płatność", "Uwzględnić rabat 10%" };
            private string[] randomCurrencies = { "zł", "PLN" };

            private string[] randomItemNames = { "Przedmiot 1", "Przedmiot 2", "Przedmiot 3", "Przedmiot 4" };

            private string paymentType;
            private DateTime baseDate;
            private int vatTax;
            private string currency;

            private int price;
            private int qty;

            private decimal totalGross;
            private decimal totalNet;

            private string PickRandomStringFromAnArray(string[] possiblePicks)
            {
                return possiblePicks[_random.Next(possiblePicks.Length)];
            }

            public RandomInvoicePlaceholderValueGenerator()
            {
                _random = new Random();

                paymentType = PickRandomStringFromAnArray(randomPaymentMethod);
                baseDate = DateTime.Now.AddDays(_random.Next(-1000, 1000));
                vatTax = 23;
                currency = PickRandomStringFromAnArray(randomCurrencies);

                GeneratedValues = new Dictionary<PlaceholderValueType, Func<string>>()
                {
                    { PlaceholderValueType.FontFamily, () => PickRandomStringFromAnArray(randomBasicFontCss) },
                    { PlaceholderValueType.Padding, () =>_random.Next(10, 40).ToString() },
                    { PlaceholderValueType.TableWidth, () => _random.Next(80,97).ToString() },
                    { PlaceholderValueType.TableBorder, () => PickRandomStringFromAnArray(randomBorderCss) },
                    { PlaceholderValueType.TablePadding, () => _random.Next(5,30).ToString() },

                    { PlaceholderValueType.InvoiceNumber, () => PickRandomStringFromAnArray(randomInvoicePrefix) + "/" + _random.Next(10,9999).ToString() + "/" + _random.Next(10000,999999).ToString() },
                    { PlaceholderValueType.NameAndSurname, () => PickRandomStringFromAnArray(randomName) + " " + PickRandomStringFromAnArray(randomSurname) },
                    { PlaceholderValueType.FullAddress,
                        () => PickRandomStringFromAnArray(randomStreet) + " "  +
                        _random.Next(1,200) + "/" + _random.Next(1,100) + ", " +
                        PickRandomStringFromAnArray(randomCity) + " " +
                        _random.Next(10,99) + "-" + _random.Next(100,999)
                    },

                    { PlaceholderValueType.NIP, () => _random.NextInt64(1000000000,9999999999).ToString() },
                    { PlaceholderValueType.BankNumber, () => PickRandomStringFromAnArray(randomBankNumber) },
                    { PlaceholderValueType.VatPercent, () => vatTax.ToString() + " %" },
                    { PlaceholderValueType.PaymentMethod, () => paymentType },
                    { PlaceholderValueType.Currency, () => currency },

                    { PlaceholderValueType.TotalGrossValue, () => totalGross.ToString() },
                    { PlaceholderValueType.TotalNetValue, () => totalNet.ToString() },

                    { PlaceholderValueType.Date, () => baseDate.ToString() },
                    { PlaceholderValueType.DeliveryDate, () => baseDate.ToString() },
                    { PlaceholderValueType.PaymentDate, () => paymentType.ToLower().Contains("przelew") ? baseDate.AddDays(_random.Next(2) == 1 ? 10 : 14).ToString() : baseDate.ToString() },

                    { PlaceholderValueType.Notes, () => PickRandomStringFromAnArray(randomNotes) }
                };
            }
            public int RandomPositiveInt(int upTo = 5)
            {
                return _random.Next(1, upTo);
            }

            public void GenerateItemPlaceholderValues()
            {
                qty = _random.Next(1, 20);
                price = _random.Next(1, 2000);

                GeneratedValues[PlaceholderValueType.ItemDescription] = () => PickRandomStringFromAnArray(randomItemNames);
                GeneratedValues[PlaceholderValueType.ItemPrice] = () => price.ToString() + " " + currency;
                GeneratedValues[PlaceholderValueType.ItemQuantity] = () => qty.ToString();
                GeneratedValues[PlaceholderValueType.GrossValue] = () => (price * qty).ToString() + " " + currency;
                GeneratedValues[PlaceholderValueType.NetValue] = () => ((price * qty) * vatTax / 100).ToString() + " " + currency;

                totalGross += price * qty;
                totalNet += (price * qty) * vatTax / 100;
            }
        }

        private static GeneratorTemplate defaultTemplate = new GeneratorTemplate()
        {
            StartTemplate =
                @"<!DOCTYPE html>
                <html lang=""pl"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Polish Invoice</title>
                    <style>
                        body {
                            font-family: {{fontFamily}};
                            padding: {{padding}}px;
                            width: 210mm;
                            height: 297mm;
                            margin: 0 auto;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }
                        table {
                            width: {{tableWidth}}%;
                            border-collapse: collapse;
                        }
                        table, th, td {
                            border: {{tableBorder}};
                        }
                        th, td {
                            padding: {{tablePadding}}px;
                            text-align: left;
                        }
                    </style>
                </head>
                <body>
                <h1>Faktura VAT nr: {{invoiceNumber}}</h1>
                <section>
                    <div>
                        <h3>Nabywca:</h3>
                        <p>{{buyersName}}</p>
                        <p>{{buyersAddress}}</p>
                        <p>NIP: {{buyersTaxID}}</p>
                    </div>
                    <div style=""float: right;"">
                        <h3>Sprzedawca:</h3>
                        <p>{{sellersName}}</p>
                        <p>{{sellersAddress}}</p>
                        <p>NIP: {{sellersTaxID}}</p>
                    </div>
                    <div style=""clear: both;""></div>
                </section>

                <h2>Przedmioty faktury:</h2>
                <table>
                <th>Lp.</th>
                <th>Opis</th>
                <th>Ilość</th>
                <th>Cena</th>
                <th>Wartość netto</th>
                <th>Podatek VAT</th>
                <th>Wartość brutto</th>",
            ItemTemplate =
                @"<tr>
                    <td>1</td>
                    <td>{{itemDescription}}</td>
                    <td>{{itemQuantity}}</td>
                    <td>{{unitPrice}}</td>
                    <td>{{netValue}}</td>
                    <td>{{vatPercent}}</td>
                    <td>{{grossValue}}</td>
                </tr>",
            FooterTemplate =
                @"<tr>
                    <td colspan=""4"">Razem</td>
                    <td>{{totalNetValue}}</td>
                    <td>{{totalVATPercent}}</td>
                    <td>{{totalGrossValue}}</td>
                    </tr>
                </table>

                <section>
                    <h2>Szczegóły płatności:</h2>
                    <p>Termin płatności: {{paymentDueDate}}</p>
                    <p>Metoda płatności: {{paymentMethod}}</p>
                    <p>Numer konta: {{bankAccountNumber}}</p>
                </section>

                <section>
                    <h2>Uwagi:</h2>
                    <p>{{additionalNotes}}</p>
                </section>
                </body>
                </html>",
            StartPlaceholders = new Dictionary<string, PlaceholderValueType>()
            {
                {"fontFamily", PlaceholderValueType.FontFamily},
                {"padding", PlaceholderValueType.Padding},
                {"tableWidth", PlaceholderValueType.TableWidth},
                {"tableBorder", PlaceholderValueType.TableBorder},
                {"tablePadding", PlaceholderValueType.TablePadding},

                {"invoiceNumber", PlaceholderValueType.InvoiceNumber},
                {"sellersName", PlaceholderValueType.NameAndSurname},
                {"sellersAddress", PlaceholderValueType.FullAddress},
                {"sellersTaxID", PlaceholderValueType.NIP},
                {"buyersName", PlaceholderValueType.NameAndSurname},
                {"buyersAddress", PlaceholderValueType.FullAddress},
                {"buyersTaxID", PlaceholderValueType.NIP},
            },
            ItemPlaceholders = new Dictionary<string, PlaceholderValueType>()
            {
                {"itemDescription", PlaceholderValueType.ItemDescription},
                {"itemQuantity", PlaceholderValueType.ItemQuantity},
                {"unitPrice", PlaceholderValueType.ItemPrice},
                {"netValue", PlaceholderValueType.NetValue},
                {"vatPercent", PlaceholderValueType.VatPercent},
                {"grossValue", PlaceholderValueType.GrossValue},
            },
            FooterPlaceholders = new Dictionary<string, PlaceholderValueType>()
            {
                {"totalGrossValue", PlaceholderValueType.GrossValue},
                {"totalNetValue", PlaceholderValueType.NetValue},
                {"totalVATPercent", PlaceholderValueType.VatPercent},
                {"paymentMethod", PlaceholderValueType.PaymentMethod},
                {"paymentDueDate", PlaceholderValueType.PaymentDate},
                {"bankAccountNumber", PlaceholderValueType.BankNumber},
                {"additionalNotes", PlaceholderValueType.Notes}
            }
        };
        private class RandomInvoiceHtmlGenerator
        {
            private readonly GeneratorTemplate _targetTemplate;
            private readonly RandomInvoicePlaceholderValueGenerator _placeholderGenerator;
            public RandomInvoiceHtmlGenerator(GeneratorTemplate targetTemplate, RandomInvoicePlaceholderValueGenerator placeholderGenerator)
            {
                _targetTemplate = targetTemplate;
                _placeholderGenerator = placeholderGenerator;
            }

            private string ReplacePlaceholders(string htmlTemplate,
                Dictionary<string, PlaceholderValueType> replacementPairs,
                Dictionary<PlaceholderValueType, Func<string>> valuePairs)
            {
                var sb = new StringBuilder(htmlTemplate);

                foreach (var key in replacementPairs.Keys)
                {
                    var valueType = replacementPairs[key];
                    var replacement = valuePairs[valueType].Invoke();
                    sb.Replace("{{" + key + "}}", replacement);
                }

                return sb.ToString();
            }

            public string GenerateRandomInvoiceHtmlFromTemplate()
            {
                var resSb = new StringBuilder();

                resSb.Append(ReplacePlaceholders(_targetTemplate.StartTemplate,
                    _targetTemplate.StartPlaceholders,
                    _placeholderGenerator.GeneratedValues));

                var numOfItems = _placeholderGenerator.RandomPositiveInt();

                for (int i = 0; i < numOfItems; i++)
                {
                    _placeholderGenerator.GenerateItemPlaceholderValues();
                    resSb.Append(ReplacePlaceholders(_targetTemplate.ItemTemplate,
                        _targetTemplate.ItemPlaceholders,
                        _placeholderGenerator.GeneratedValues));
                }

                resSb.Append(ReplacePlaceholders(_targetTemplate.FooterTemplate,
                    _targetTemplate.FooterPlaceholders,
                    _placeholderGenerator.GeneratedValues));

                return resSb.ToString();
            }
        }

        private readonly IBrowserAutomationService _browserAutomationService;

        public InvoiceGeneratorService(IBrowserAutomationService browserAutomationService)
        {
            _browserAutomationService = browserAutomationService;
        }

        public async Task<byte[]> GenerateRandomInvoicePdf()
        {
            var htmlGenerator = new RandomInvoiceHtmlGenerator(defaultTemplate,
                new RandomInvoicePlaceholderValueGenerator());

            var invoiceHtml = htmlGenerator.GenerateRandomInvoiceHtmlFromTemplate();

            return await _browserAutomationService.GeneratePdfFromHtml(invoiceHtml);
        }
    }
}