namespace OcrInvoiceBackend.API.Configurations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ConfigurationForEnvironmentAttribute : Attribute
    {
        public string EnvironmentName { get; }

        public ConfigurationForEnvironmentAttribute(string environmentName)
        {
            EnvironmentName = environmentName;
        }
    }
}
