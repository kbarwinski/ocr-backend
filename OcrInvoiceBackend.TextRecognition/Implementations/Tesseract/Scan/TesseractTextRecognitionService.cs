using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using Tesseract;
using PDFiumSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using System.Linq.Expressions;
using OcrInvoiceBackend.Application.Services.TextRecognition;
using OcrInvoiceBackend.Domain.Entities;

namespace OcrInvoiceBackend.TextRecognition.Implementations.Tesseract.Scan
{
    public class TesseractTextRecognitionService : ITextRecognitionService
    {
        public ScanResults ScanInvoice(byte[] invoiceData)
        {

            using (var engine = new TesseractEngine(Environment.GetEnvironmentVariable("TESS_DATA_PATH"),
                Environment.GetEnvironmentVariable("TESS_LANG"), EngineMode.Default))
            {
                using (var pix = Pix.LoadFromMemory(invoiceData))
                {
                    engine.DefaultPageSegMode = PageSegMode.AutoOsd;

                    using (var page = engine.Process(pix))
                    {
                        List<ScannedWord> blocks = new List<ScannedWord>();

                        using (var iter = page.GetIterator())
                        {
                            iter.Begin();
                            var lineNumber = 0;
                            var blockNumber = 0;
                            var paraNumber = 0;
                            do
                            {
                                if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                    blockNumber++;
                                if (iter.IsAtBeginningOf(PageIteratorLevel.Para))
                                    paraNumber++;
                                if (iter.IsAtBeginningOf(PageIteratorLevel.TextLine))
                                    lineNumber++;

                                var block = iter.GetText(PageIteratorLevel.Word);
                                var certainty = iter.GetConfidence(PageIteratorLevel.Word);
                                iter.TryGetBoundingBox(PageIteratorLevel.Word, out var bounds);

                                blocks.Add(new ScannedWord
                                {
                                    BlockNumber = blockNumber,
                                    ParaNumber = paraNumber,
                                    LineNumber = lineNumber,

                                    Text = block,
                                    Certainty = certainty,
                                    Bounds = new Coords(bounds.X1, bounds.Y1, bounds.X2, bounds.Y2)
                                });
                            } while (iter.Next(PageIteratorLevel.Word));

                            var boundsData = blocks.Select(x => x.Bounds);
                            return new ScanResults()
                            {
                                Words = blocks,
                                Certainty = page.GetMeanConfidence(),
                                ImageBounds = new Coords(0, 0, pix.Width, pix.Height),
                                ContentBounds = new Coords(
                                    boundsData.Min(x => x.X1),
                                    boundsData.Min(y => y.Y1),
                                    boundsData.Max(x => x.X2),
                                    boundsData.Max(y => y.Y2))
                            };
                        }
                    }

                }
            }
        }
    }
}