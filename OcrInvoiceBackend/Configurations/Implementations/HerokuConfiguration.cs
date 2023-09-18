namespace OcrInvoiceBackend.API.Configurations.Implementations
{
    [ConfigurationForEnvironment("Production")]
    public class HerokuConfiguration : BaseConfiguration
    {
        public override void ConfigureForEnvironment(WebApplicationBuilder builder)
        {
            var envConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
            var envIdentityConnectionString = Environment.GetEnvironmentVariable("HEROKU_POSTGRESQL_RED_URL");
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");

            if (!string.IsNullOrEmpty(envConnectionString) && !string.IsNullOrEmpty(envIdentityConnectionString) && !string.IsNullOrEmpty(jwtKey))
            {
                var convertedConnectionString = ConvertConnectionString(envConnectionString);
                var convertedIdentityConnectionString = ConvertConnectionString(envIdentityConnectionString);

                var inMemoryConfig = new Dictionary<string, string>
                {
                    {"ConnectionStrings:PostgreSQL", convertedConnectionString},
                    {"ConnectionStrings:IdentityPostgreSQL", convertedIdentityConnectionString },
                    {"Jwt:Key", jwtKey},
                    {"Jwt:Issuer","OCRInvoices"}
                };

                builder.Configuration.AddInMemoryCollection(inMemoryConfig);
            }

            var portExists = int.TryParse(Environment.GetEnvironmentVariable("PORT"), out var port);
            if (portExists)
            {
                builder.WebHost.ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(port);
                });
            }
        }
    }
}
