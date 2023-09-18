namespace OcrInvoiceBackend.API.Configurations
{
    public abstract class BaseConfiguration : IEnvironmentConfiguration
    {
        protected string ConvertConnectionString(string url)
        {
            var uri = new Uri(url);
            var userInfo = uri.UserInfo.Split(':');
            return $"Server={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.Substring(1)};User Id={userInfo[0]};Password={userInfo[1]};";
        }

        public abstract void ConfigureForEnvironment(WebApplicationBuilder builder);
    }
}
