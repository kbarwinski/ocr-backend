namespace OcrInvoiceBackend.API.Configurations.Implementations
{
    [ConfigurationForEnvironment("Development")]
    public class LocalConfiguration : BaseConfiguration
    {
        public override void ConfigureForEnvironment(WebApplicationBuilder builder)
        {
            DotNetEnv.Env.Load("../.env");

            var envConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
            var envIdentityConnectionString = Environment.GetEnvironmentVariable("IDENTITY_DATABASE_URL");
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            var endpointPort = Environment.GetEnvironmentVariable("MAPPING_PORT");

            if (!string.IsNullOrEmpty(envConnectionString) && !string.IsNullOrEmpty(envIdentityConnectionString) && !string.IsNullOrEmpty(jwtKey))
            {
                var inMemoryConfig = new Dictionary<string, string>
                {
                    {"ConnectionStrings:PostgreSQL", envConnectionString},
                    {"ConnectionStrings:IdentityPostgreSQL", envIdentityConnectionString },
                    {"Jwt:Key", jwtKey},
                    {"Jwt:Issuer","OCRInvoices"},
                    {"Kestrel:EndPoints:Http:Url","http://localhost:" + endpointPort}
                };

                builder.Configuration.AddInMemoryCollection(inMemoryConfig);
            }
        }
    }
}
