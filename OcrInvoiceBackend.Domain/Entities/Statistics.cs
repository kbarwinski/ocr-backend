using OcrInvoiceBackend.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Domain.Entities
{
    public class Statistics : BaseEntity
    {
        [ConcurrencyCheck]
        public int Version { get; set; }

        public int InvoicesUploaded { get; set; }

        public double TotalUploadTime { get; set; }
        public double AverageUploadTime { get; set; }

        public int InvoicesScanned { get; set; }

        public double TotalScanTime { get; set; }
        public double AverageScanTime { get; set; }

        public double TotalScanCertainty { get; set; }
        public double AverageScanCertainty { get; set; }

        public int InvoicesParsed { get; set; }
        public int DetailsParsed { get; set; }

        public double TotalParsingTime { get; set; }
        public double AverageParsingTime { get; set; }

        public double TotalParsingCertainty { get; set; }
        public double AverageParsingCertainty { get; set; }

        public int InvoicesApproved { get; set; }
        public int DetailsApproved { get; set; }

        public int DetailsGuessed { get; set; }
        public int DetailsCorrected { get; set; }
    }
}
