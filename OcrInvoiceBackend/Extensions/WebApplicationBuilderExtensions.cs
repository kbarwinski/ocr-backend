using System.Reflection;
using OcrInvoiceBackend.API.Configurations;

namespace OcrInvoiceBackend.API.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void ConfigureForEnvironment(this WebApplicationBuilder builder)
        {
            var currentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
           
            var configurations = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(BaseConfiguration).IsAssignableFrom(t) && !t.IsAbstract)
                .Select(Activator.CreateInstance)
                .OfType<BaseConfiguration>()
                .ToList();

            foreach (var config in configurations)
            {
                var environmentAttribute = config.GetType().GetCustomAttribute<ConfigurationForEnvironmentAttribute>();
                if (environmentAttribute != null && environmentAttribute.EnvironmentName == currentEnvironment)
                {
                    config.ConfigureForEnvironment(builder);
                    return;
                }
            }

            throw new Exception($"No configuration found for environment {currentEnvironment}");
        }
    }
}
