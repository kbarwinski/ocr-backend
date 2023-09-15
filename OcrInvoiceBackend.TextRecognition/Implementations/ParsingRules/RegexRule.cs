using OcrInvoiceBackend.Application.Services.TextRecognition;
using System.Text.RegularExpressions;

namespace OcrInvoiceBackend.TextRecognition.Implementations.ParsingRules
{
    public class RegexRule : IParsingRule
    {
        public string Name => "Regex rule";
        public string RegexPhrase { get; set; }

        public RegexRule(string regex)
        {
            RegexPhrase = regex;
        }

        public ParseResults? Parse(string input)
        {
            if (Regex.IsMatch(input, RegexPhrase))
            {
                var match = Regex.Match(input, RegexPhrase);
        
                return new ParseResults(match.Index, match.ToString(), 1 );
            }

            return null;
        }
    }
}
