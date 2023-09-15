using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models
{
    public class InvoiceUpdateModel
    {
        public string Name { get; set; }
        public bool IsScanned { get; set; }
        public bool IsParsed { get; set; }
        public bool IsApproved { get; set; }
    }
}
