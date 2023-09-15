using OcrInvoiceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models
{
    public class DetailDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public float Certainty { get; set; }
        public Guid InvoiceId { get; set; }
    }
}
