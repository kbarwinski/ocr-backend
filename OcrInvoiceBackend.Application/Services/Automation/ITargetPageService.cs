using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Services.Automation
{
    public interface IPreparationArgs
    {
    }

    public interface ITargetPageService
    {
        string TargetPageName { get; }
        Task PrepareTargetPage(IPreparationArgs args);
    }
}
