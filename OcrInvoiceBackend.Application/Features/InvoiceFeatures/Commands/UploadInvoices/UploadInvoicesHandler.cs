using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;
using OcrInvoiceBackend.Application.Repositories;
using OcrInvoiceBackend.Application.Services.BackgroundQueue;
using OcrInvoiceBackend.Application.Services.TextRecognition;
using OcrInvoiceBackend.Domain.Entities;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.UploadInvoices
{
    public sealed class UploadInvoicesHandler : IRequestHandler<UploadInvoicesCommand>
    {
        private readonly IBackgroundQueue _backgroundQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly ILogger<UploadInvoicesHandler> _logger;

        public UploadInvoicesHandler(
            ILogger<UploadInvoicesHandler> logger,
            IBackgroundQueue backgroundQueue,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _backgroundQueue = backgroundQueue;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Handle(UploadInvoicesCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Uploading process had started.");

            var fileDataList = new List<byte[]>();
            foreach (var file in request.Files)
            {

                using var stream = new MemoryStream();
                await file.CopyToAsync(stream, cancellationToken);
                fileDataList.Add(stream.ToArray());
                _logger.LogInformation(file.FileName + "copied and added to an array.");

            }

            _backgroundQueue.EnqueueAsync(async token =>
            {
                using var scope = _serviceScopeFactory.CreateScope();

                var logger = scope.ServiceProvider.GetRequiredService<ILogger<UploadInvoicesHandler>>();

                logger.LogInformation("Background upload had started.");

                var invoiceRepository = scope.ServiceProvider.GetRequiredService<IInvoiceRepository>();
                var statisticsRepository = scope.ServiceProvider.GetRequiredService<IStatisticsRepository>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var imageManipulatorService = scope.ServiceProvider.GetRequiredService<IImageManipulatorService>();

                var startUploadTime = DateTime.Now;

                var invoiceEntities = new List<Invoice>();

                foreach (var fileData in fileDataList)
                {
                    logger.LogInformation("Handling file upload in the background had started.");

                    var cfg = imageManipulatorService.DefaultCfg;
                    cfg.FileExtension = request.Type.ToString();

                    invoiceEntities.Add(new Invoice
                    {
                        Name = DateTime.Now.ToString(),
                        FileData = imageManipulatorService.PrepareFile(fileData, cfg),
                        FileType = FileType.JPG,
                    });

                    logger.LogInformation("Handling file upload in the background had ended.");
                }


                logger.LogInformation("Context created multiple invoices.");

                invoiceRepository.CreateRange(invoiceEntities);

                var endUploadTime = DateTime.Now;

                var totalUploadTime = (endUploadTime - startUploadTime).TotalSeconds;

                var todayStats = await statisticsRepository.GetTodayStatistics();

                todayStats.InvoicesUploaded += invoiceEntities.Count;

                todayStats.TotalUploadTime += totalUploadTime;
                todayStats.AverageUploadTime = todayStats.TotalUploadTime / todayStats.InvoicesUploaded;

                statisticsRepository.Update(todayStats);

                await unitOfWork.Save(cancellationToken);

                logger.LogInformation("Context saved.");

                return "UploadCompleted";
            });
        }
    }
}
