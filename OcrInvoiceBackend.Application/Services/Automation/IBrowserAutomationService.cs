using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Services.Automation
{
    public interface IBrowserAutomationService
    {
        string CurrentPageUrl { get; }
        string ProviderName { get; }

        Task<byte[]> GeneratePdfFromHtml(string rawHtml);
        Task InitializeService();
        Task NavigateToPage(string pageUrl);

        Task SelectAndClick(string selector);
        Task SelectAndType(string selector, string text);
    }
}
