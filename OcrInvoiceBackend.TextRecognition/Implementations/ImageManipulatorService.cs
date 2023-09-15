using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using PDFiumSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using OcrInvoiceBackend.Application.Services.TextRecognition;

namespace OcrInvoiceBackend.TextRecognition.Implementations
{
    public class ImageManipulatorService : IImageManipulatorService
    {
        ImageConfiguration IImageManipulatorService.DefaultCfg => new ImageConfiguration()
        {
            FileExtension = "",
            TargetWidth = 423,
            TargetHeight = 300,
            TargetDPI = 300,
            TargetScale = 6,
        };

        private byte[] PdfToJpeg(byte[] rawPdf, ImageConfiguration cfg)
        {
            using var pdfDocument = new PdfDocument(rawPdf);
            var firstPage = pdfDocument.Pages[0];

            using var pageBitmap = new PDFiumBitmap(cfg.TargetHeight * cfg.TargetScale, cfg.TargetWidth * cfg.TargetScale, true);

            firstPage.Render(pageBitmap);

            var image = Image.Load(pageBitmap.AsBmpStream());
            image.Mutate(x => x.BackgroundColor(Rgba32.ParseHex("FFF")));

            using (var ms = new MemoryStream())
            {
                image.Save(ms, new JpegEncoder());
                return ms.ToArray();
            }
        }

        public byte[] PrepareFile(byte[] input, ImageConfiguration cfg)
        {
            var cfgImpl = cfg as ImageConfiguration;
            if (cfgImpl == null)
                throw new NullReferenceException();

            var res = input;
            if (cfgImpl.FileExtension.ToLower().Contains("pdf"))
            {
                res = PdfToJpeg(input, cfgImpl);
            }

            return res;
        }
    }
}
