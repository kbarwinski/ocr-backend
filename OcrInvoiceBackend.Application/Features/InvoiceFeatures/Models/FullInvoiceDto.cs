using OcrInvoiceBackend.Domain.Entities;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models
{
    public sealed record FullInvoiceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<DetailDto> Details { get; set; }
        public bool IsScanned { get; set; } = false;
        public bool IsParsed { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public Byte[] FileData { get; set; }
        public FileType FileType { get; set; }
    }
}
