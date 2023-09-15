using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Services.TextRecognition
{
    public class ParseResults
    {
        public ParseResults(int foundIndex, string foundText, float certainty)
        {
            FoundIndex = foundIndex;
            FoundText = foundText;
            Certainty = certainty;
        }

        public int FoundIndex { get; set; }
        public string FoundText { get; set; }
        public float Certainty { get; set; }
    }

    public interface IParsingRule
    {
        string Name { get; }
        ParseResults? Parse(string input);
    }
}
