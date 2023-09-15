using Microsoft.Extensions.Logging;
using OcrInvoiceBackend.Application.Services.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Automation.Implementations.TargetPages
{
    public interface ISaldeoPreparationArgs : IPreparationArgs
    {
        public string Login { get; }
        public string Password { get; }
    }

    public class SaldeoTargetPageService : ITargetPageService
    {
        private readonly IBrowserAutomationService _automationService;
        private readonly ILogger _logger;

        public SaldeoTargetPageService(IBrowserAutomationService automationService, ILogger<SaldeoTargetPageService> logger)
        {
            _automationService = automationService;
            _logger = logger;
        }

        public string TargetPageName => "Saldeo";

        public async Task PrepareTargetPage(IPreparationArgs args)
        {
            await _automationService.InitializeService();
            await _automationService.NavigateToPage("https://saldeo.brainshare.pl/");

            var properArgs = args as ISaldeoPreparationArgs;
            if (properArgs == null)
                throw new Exception();

            _logger.LogDebug("Login: " + properArgs.Login);
            _logger.LogDebug("Password: " + properArgs.Password);

            await _automationService.SelectAndType("[id='j_id54:userName']", properArgs.Login);
            await _automationService.SelectAndType("[id='j_id54:password']", properArgs.Password);

            await _automationService.SelectAndClick("[id='j_id54:login']");
        }
    }
}
