using Newtonsoft.Json;
using OcrInvoiceBackend.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Domain.Entities
{
    public class Coords
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }

        public Coords(int x1, int y1, int x2, int y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public int Width => X2 - X1;
        public int Height => Y2 - Y1;
        public float Area => Width * Height;
    }

    public class ScannedWord
    {
        public int BlockNumber { get; set; }
        public int ParaNumber { get; set; }
        public int LineNumber { get; set; }

        public string Text { get; set; }
        public float Certainty { get; set; }
        public Coords Bounds { get; set; }
    }

    public class ScanResults : BaseEntity
    {
        public string WordsJson { get; set; }
        public float Certainty { get; set; }
        public Coords ImageBounds { get; set; }
        public Coords ContentBounds { get; set; }

        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;

        [NotMapped]
        public List<ScannedWord> Words
        {
            get => JsonConvert.DeserializeObject<List<ScannedWord>>(WordsJson);
            set => WordsJson = JsonConvert.SerializeObject(value);
        }

        public string RawTextWhere(Func<ScannedWord, bool> predicate)
        {
            var words = Words
                .Where(predicate)
                .OrderBy(x => x.LineNumber)
                .ToList();

            var res = words[0].Text;
            var currLine = 0;
            for (int i = 1; i < words.Count; i++)
            {
                var word = words.ElementAt(i);
                if (word.LineNumber > currLine)
                {
                    res += " \\n ";
                    currLine = word.LineNumber;
                }
                else
                    res += " ";
                res += word.Text;
            }

            return res;
        }
    }
}
