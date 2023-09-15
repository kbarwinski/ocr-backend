using OcrInvoiceBackend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Domain.Entities
{
    public class Detail : BaseEntity
    {
        public string Name { get; set; }
        public string? Value { get; set; }
        public float Certainty { get; set; }

        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;
    }
}
