using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Services.TextRecognition
{
    public class ImageConfiguration
    {
        public string FileExtension { get; set; }
        public int TargetDPI { get; set; }
        public int TargetScale { get; set; }
        public int TargetWidth { get; set; }
        public int TargetHeight { get; set; }
    };

    public interface IImageManipulatorService
    {
        ImageConfiguration DefaultCfg { get; }
        Byte[] PrepareFile(Byte[] input, ImageConfiguration cfg);
    }
}
