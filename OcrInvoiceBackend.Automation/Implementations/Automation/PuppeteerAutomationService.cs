using OcrInvoiceBackend.Application.Services.Automation;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace OcrInvoiceBackend.Implementations.Automation
{
    public class PuppeteerAutomationService : IBrowserAutomationService
    {
        public IBrowser BrowserInstance { get; set; }

        public string ProviderName => "Puppeteer";

        public string CurrentPageUrl { get; set; }
        public IPage CurrentPage { get; set; }

        public List<string> PageHistory { get; set; }

        public PuppeteerAutomationService()
        {
            if (BrowserInstance == null)
                InitializeService().GetAwaiter().GetResult();
        }

        public async Task InitializeService()
        {
            PageHistory = new List<string>();

            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

            var opts = new LaunchOptions() { Headless = false };

            BrowserInstance = await Puppeteer.LaunchAsync(opts);
            CurrentPage = await BrowserInstance.NewPageAsync();
        }

        private static readonly PdfOptions DefaultOptions = new PdfOptions
        {
            Format = PaperFormat.A4,
            PrintBackground = true
        };

        public async Task<byte[]> GeneratePdfFromHtml(string rawHtml)
        {
            var options = DefaultOptions;

            var page = await BrowserInstance.NewPageAsync();
            await page.SetContentAsync(rawHtml);

            var pdfBytes = await page.PdfDataAsync(options);

            await page.CloseAsync();

            return pdfBytes;
        }

        public async Task NavigateToPage(string pageUrl)
        {
            if (CurrentPageUrl != null && CurrentPageUrl.Length > 0)
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
