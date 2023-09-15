using Newtonsoft.Json.Linq;
using OcrInvoiceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models
{
    public sealed record InvoiceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsScanned { get; set; }
        public bool IsParsed { get; set; }
        public bool IsApproved { get; set; }
    }
}
