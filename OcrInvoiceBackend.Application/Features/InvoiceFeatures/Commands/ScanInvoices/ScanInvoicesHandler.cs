using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.ScanInvoice;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;
using OcrInvoiceBackend.Application.Repositories;
using OcrInvoiceBackend.Application.Services.BackgroundQueue;
using OcrInvoiceBackend.Application.Services.TextRecognition;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.ScanInvoices
{
    public class ScanInvoicesHandler : IRequestHandler<ScanInvoicesCommand>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IBackgroundQueue _backgroundQueue;

        public ScanInvoicesHandler(IServiceScopeFactory scopeFactory,
            IBackgroundQueue backgroundQueue)
        {
            _scopeFactory = scopeFactory;
            _backgroundQueue = backgroundQueue;
        }

        public async Task Handle(ScanInvoicesCommand request, CancellationToken cancellationToken)
        {
            _backgroundQueue.EnqueueAsync(async token =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var invoiceRepository = scope.ServiceProvider.GetRequiredService<IInvoiceRepository>();
                    var textRecognitionService = scope.ServiceProvider.GetRequiredService<ITextRecognitionService>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var statisticsRepository = scope.ServiceProvider.GetRequiredService<IStatisticsRepository>();

                    var guids = request.InvoiceIds
                        .Select(x => new Guid(x))
                        .ToList();

                    var entities = await invoiceRepository.GetByIds(guids, cancellationToken);

                    var todayStats = await statisticsRepository.GetTodayStatistics();

                    foreach (var entity in entities)
                    {
                        var startTime = DateTime.Now;
                        var scanResults = textRecognitionService.ScanInvoice(entity.FileData);
                        var endTime = DateTime.Now;

                        entity.Scan = scanResults;
                        entity.IsScanned = true;

                        todayStats.InvoicesScanned += 1;

                        todayStats.TotalScanTime += (endTime - startTime).TotalSeconds;
                        todayStats.AverageScanTime = todayStats.TotalScanTime / todayStats.InvoicesScanned;

                        todayStats.TotalScanCertainty += scanResults.Certainty;
                        todayStats.AverageScanCertainty = todayStats.TotalScanCertainty / todayStats.InvoicesScanned;
                    }

                    statisticsRepository.Update(todayStats);
                    invoiceRepository.UpdateRange(entities);

                    await unitOfWork.Save(cancellationToken);

                    return "BatchScanCompleted";
                }
            });
        }
    }
}
