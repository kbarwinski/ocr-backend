using Microsoft.Extensions.DependencyInjection;
using OcrInvoiceBackend.Application.Repositories;
using OcrInvoiceBackend.Persistence.Context;
using OcrInvoiceBackend.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FluentMigrator.Runner;
using OcrInvoiceBackend.Persistence.Migrations;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Generators;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using OcrInvoiceBackend.Persistence.Migrations.Identity;

namespace OcrInvoiceBackend.Persistence
{
    public static class ServiceExtensions
    {
        private static readonly List<ServiceProvider> _migrationProviders = new();

        public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgreSQL");
            services.AddDbContext<DataContext>(opt => opt.UseNpgsql(connectionString));

            services.AddSingleton<IParsingFieldRepository, ParsingFieldRepository>();

            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IScanResultsRepository, ScanResultsRepository>();
            services.AddScoped<IDetailRepository, DetailRepository>();

            services.AddScoped<IStatisticsRepository, StatisticsRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            _migrationProviders.Add(services.AddFluentMigratorCore()
                .ConfigureRunner(cfg => cfg
                    .AddPostgres()
                    .ScanIn(typeof(BaseMigration).Assembly).For.Migrations())
                    .Configure<SelectingProcessorAccessorOptions>(cfg => cfg.ProcessorId = "PostgreSQL")
                    .Configure<ProcessorOptions>(cfg => cfg.ConnectionString = connectionString)
                .BuildServiceProvider());

            var identityConnectionString = configuration.GetConnectionString("IdentityPostgreSQL");

            _migrationProviders.Add(services.AddFluentMigratorCore()
                .ConfigureRunner(cfg => cfg
                        .AddPostgres()
                        .ScanIn(typeof(IdentityBaseMigration).Assembly).For.Migrations())
                        .Configure<SelectingProcessorAccessorOptions>(cfg => cfg.ProcessorId = "PostgreSQL")
                        .Configure<ProcessorOptions>(cfg => cfg.ConnectionString = identityConnectionString)
                .BuildServiceProvider());
        }

        public static void UpdateDatabases(this IApplicationBuilder app)
        {
            foreach (var serviceProvider in _migrationProviders)
            {
                var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
        }
    }
}
