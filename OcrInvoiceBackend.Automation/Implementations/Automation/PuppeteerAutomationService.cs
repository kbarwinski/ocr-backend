using OcrInvoiceBackend.Application.Services.Automation;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Implementations.Automation
{
    public class PuppeteerAutomationService : IBrowserAutomationService
    {
        public IBrowser BrowserInstance { get; set; }

        public string ProviderName => "Puppeteer";

        public string CurrentPageUrl { get; set; }
        public IPage CurrentPage { get; set; }

        public List<String> PageHistory { get; set; }

        public async Task InitializeService()
        {
            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

            var opts = new LaunchOptions() { Headless = false };

            BrowserInstance = await Puppeteer.LaunchAsync(opts);
            CurrentPage = await BrowserInstance.NewPageAsync();
        }


        public async Task NavigateToPage(string pageUrl)
        {
            if (CurrentPageUrl != null && CurrentPageUrl.Length>0)
                PageHistory.Add(CurrentPageUrl);

            CurrentPageUrl = pageUrl;
            await CurrentPage.GoToAsync(pageUrl);
        }

        public async Task SelectAndClick(string selector)
        {
            await CurrentPage.WaitForSelectorAsync(selector);
            await CurrentPage.ClickAsync(selector);
        }

        public async Task SelectAndType(string selector, string text)
        {
            await CurrentPage.WaitForSelectorAsync(selector);
            await CurrentPage.TypeAsync(selector, text);
        }
    }
}
