using OcrInvoiceBackend.TextRecognition.Implementations.Tesseract.Scan;
using OcrInvoiceBackend.TextRecognition.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OcrInvoiceBackend.Application.Services.TextRecognition;
using System.Reflection;

namespace OcrInvoiceBackend.TextRecognition
{
    public static class ServiceExtensions
    {
        public static void ConfigureTextRecognition(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IImageManipulatorService, ImageManipulatorService>();
            services.AddScoped<ITextRecognitionService, TesseractTextRecognitionService>();

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IParsingField)) && !t.IsInterface && !t.IsAbstract);
            foreach (var type in types)
            {
                services.AddTransient(typeof(IParsingField), type);
            }
        }
    }
}
