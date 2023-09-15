using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OcrInvoiceBackend.Application.Services.Automation;
using OcrInvoiceBackend.Automation.Implementations.TargetPages;
using OcrInvoiceBackend.Implementations.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Automation
{
    public static class ServiceExtensions
    {
        public static void ConfigureAutomation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IBrowserAutomationService, PuppeteerAutomationService>();
            services.AddSingleton<ITargetPageService, SaldeoTargetPageService>();
        }
    }
}
