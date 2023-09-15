using OcrInvoiceBackend.Application.Services.TextRecognition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.TextRecognition.Implementations.ParsingRules
{
    public class HammingSimilarityRule : IParsingRule
    {
        public string Name => "Similarity rule";
        public string TargetWord { get; set; }

        public HammingSimilarityRule(string targetWord)
        {
            TargetWord = targetWord;
        }

        private int GetHammingDistance(string s, string t)
        {
            if (s.Length != t.Length)
            {
                throw new Exception("Strings must be equal length");
            }

            int distance =
                s.ToCharArray()
                .Zip(t.ToCharArray(), (c1, c2) => new { c1, c2 })
                .Count(m => m.c1 != m.c2);

            return distance;
        }

        public ParseResults? Parse(string input)
        {
            var word = TargetWord;

            var wordLen = word.Length;
            var resultWord = "";
            var resultIndex = -1;

            float maxSimilarity = 0;
            for (int i = 0; i <= input.Length - wordLen; i++)
            {
                var textPart = input.Substring(i, wordLen);
                float similarity = (wordLen - GetHammingDistance(textPart, word)) / (float)wordLen;
                if (similarity >= maxSimilarity)
                {
                    maxSimilarity = similarity;
                    resultWord = textPart;
                    resultIndex = i;
                }
            }

            if (maxSimilarity <= 0)
                return null;

            return new ParseResults(resultIndex, resultWord, maxSimilarity);
        }
    }
}
