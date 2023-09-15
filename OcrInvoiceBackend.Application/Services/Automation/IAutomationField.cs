using OcrInvoiceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Services.Automation
{
    public interface IAutomationField
    {
        void ExecuteAutomation(Detail toFill);
    }
}
