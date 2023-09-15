using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
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

        public UploadInvoicesHandler(
            IMapper mapper,
            IBackgroundQueue backgroundQueue,
            IServiceScopeFactory serviceScopeFactory)
        {
            _backgroundQueue = backgroundQueue;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Handle(UploadInvoicesCommand request, CancellationToken cancellationToken)
        {
            var fileDataList = new List<byte[]>();
            foreach (var file in request.Files)
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream, cancellationToken);
                fileDataList.Add(stream.ToArray());
            }

            _backgroundQueue.EnqueueAsync(async token =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var invoiceRepository = scope.ServiceProvider.GetRequiredService<IInvoiceRepository>();
                var statisticsRepository = scope.ServiceProvider.GetRequiredService<IStatisticsRepository>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var imageManipulatorService = scope.ServiceProvider.GetRequiredService<IImageManipulatorService>();

                var startUploadTime = DateTime.Now;

                var invoiceEntities = new List<Invoice>();

                foreach (var fileData in fileDataList)
                {
                    var cfg = imageManipulatorService.DefaultCfg;
                    cfg.FileExtension = request.Type.ToString();

                    invoiceEntities.Add(new Invoice
                    {
                        Name = DateTime.Now.ToString(),
                        FileData = imageManipulatorService.PrepareFile(fileData, cfg),
                        FileType = FileType.JPG,
                    });
                }

                invoiceRepository.CreateRange(invoiceEntities);

                var endUploadTime = DateTime.Now;

                var totalUploadTime = (endUploadTime - startUploadTime).TotalSeconds;

                var todayStats = await statisticsRepository.GetTodayStatistics();

                todayStats.InvoicesUploaded += invoiceEntities.Count;

                todayStats.TotalUploadTime += totalUploadTime;
                todayStats.AverageUploadTime = todayStats.TotalUploadTime / todayStats.InvoicesUploaded;

                statisticsRepository.Update(todayStats);
                await unitOfWork.Save(cancellationToken);

                return "UploadCompleted";
            });
        }
    }
}
