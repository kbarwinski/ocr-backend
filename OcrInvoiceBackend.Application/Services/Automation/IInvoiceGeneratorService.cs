using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Services.Automation
{
    public interface IInvoiceGeneratorService
    {
        Task<byte[]> GenerateRandomInvoicePdf();
    }
}
