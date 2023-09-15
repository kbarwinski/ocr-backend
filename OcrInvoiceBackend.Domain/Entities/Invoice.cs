using OcrInvoiceBackend.Domain.Common;

namespace OcrInvoiceBackend.Domain.Entities
{
    public enum FileType
    {
        PDF = 1,
        JPG = 2,
        PNG = 3,
    }

    public sealed class Invoice : BaseEntity
    {
        public string Name { get; set; }

        public byte[] FileData { get; set; }
        public FileType FileType { get; set; }

        public ScanResults? Scan { get; set; }

        public bool IsScanned { get; set; } = false;
        public bool IsParsed { get; set; } = false;
        public bool IsApproved { get; set; } = false;

        public ICollection<Detail> Details { get; set; } = new List<Detail>();
    }
}
