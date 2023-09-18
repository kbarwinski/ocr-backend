namespace OcrInvoiceBackend.API.Configurations
{
    public interface IEnvironmentConfiguration
    {
        void ConfigureForEnvironment(WebApplicationBuilder builder);
    }
}
