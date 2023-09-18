using OcrInvoiceBackend.API.Extensions;
using OcrInvoiceBackend.Application;
using OcrInvoiceBackend.Automation;
using OcrInvoiceBackend.TextRecognition;
using OcrInvoiceBackend.Persistence;
using OcrInvoiceBackend.Persistence.Context;
using OcrInvoiceBackend.Infrastructure;
using OcrInvoiceBackend.Infrastructure.Identity;
using OcrInvoiceBackend.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureForEnvironment();

builder.Services.AddLogging();

builder.Services.ConfigureApplication();

builder.Services.ConfigureInfrastructure(builder.Configuration);
builder.Services.ConfigureIdentity(builder.Configuration);

builder.Services.ConfigureAutomation(builder.Configuration);
builder.Services.ConfigureTextRecognition(builder.Configuration);

builder.Services.ConfigurePersistence(builder.Configuration);

builder.Services.ConfigureApiBehavior();
builder.Services.ConfigureCorsPolicy();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials() // Required for SignalR
            .SetIsOriginAllowed(host => true)); // Allow any origin
});

var app = builder.Build();

var serviceScope = app.Services.CreateScope();

var dataContext = serviceScope.ServiceProvider.GetService<DataContext>();
dataContext?.Database.EnsureCreated();

var identityContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
identityContext?.Database.EnsureCreated();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseErrorHandler();

app.UseCors("CorsPolicy");

app.UseIdentity(app.Environment);

app.UseMiddleware<JwtMiddleware>();

app.UpdateDatabases();

app.UseSignalR(app.Environment);

app.MapControllers();

app.Run();