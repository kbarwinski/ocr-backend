using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OcrInvoiceBackend.Application.Common.Exceptions;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.AnalyzeInvoice;
using OcrInvoiceBackend.Application.Repositories;
using OcrInvoiceBackend.Application.Services.BackgroundQueue;
using OcrInvoiceBackend.Application.Services.TextRecognition;
using OcrInvoiceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.AnalyzeInvoices
{
    public class AnalyzeInvoicesHandler : IRequestHandler<AnalyzeInvoicesCommand>
    {
        private readonly IBackgroundQueue _backgroundQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AnalyzeInvoicesHandler(IBackgroundQueue backgroundQueue, IServiceScopeFactory serviceScopeFactory)
        {
            _backgroundQueue = backgroundQueue;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Handle(AnalyzeInvoicesCommand request, CancellationToken cancellationToken)
        {
            _backgroundQueue.EnqueueAsync(async token =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var invoiceRepository = scope.ServiceProvider.GetRequiredService<IInvoiceRepository>();
                    var detailRepository = scope.ServiceProvider.GetRequiredService<IDetailRepository>();
                    var parsingFieldRepository = scope.ServiceProvider.GetRequiredService<IParsingFieldRepository>();
                    var statisticsRepository = scope.ServiceProvider.GetRequiredService<IStatisticsRepository>();

                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    List<Guid> guids = request.Ids
                        .Select(x => new Guid(x))
                        .ToList();

                    var entities = await invoiceRepository.GetByIds(guids, cancellationToken);
                    var todayStats = await statisticsRepository.GetTodayStatistics();

                    int totalDetailsParsed = 0;
                    double totalParsingTime = 0;
                    double totalParsingCertainty = 0;

                    foreach (var entity in entities)
                    {
                        var parsingStartTime = DateTime.Now;

                        var availableFields = await parsingFieldRepository
                            .GetByCondition(x => true, cancellationToken);

                        var detailTasks = availableFields.Select(detail => detail.ParseForField(entity.Scan)).ToList();

                        var results = (await Task.WhenAll(detailTasks)).ToList();

                        foreach (var field in availableFields)
                            field.DeduceFromResults(results);

                        entity.Details = results;
                        entity.IsParsed = true;

                        var parsingEndTime = DateTime.Now;
                        totalParsingTime += (parsingEndTime - parsingStartTime).TotalSeconds;
                        totalDetailsParsed += results.Count;
                        totalParsingCertainty += results.Sum(result => result.Certainty);
                    }

                    todayStats.InvoicesParsed += entities.Count;
                    todayStats.DetailsParsed += totalDetailsParsed;

                    todayStats.TotalParsingTime += totalParsingTime;
                    todayStats.AverageParsingTime = todayStats.TotalParsingTime / todayStats.InvoicesParsed;

                    todayStats.TotalParsingCertainty += totalParsingCertainty;
                    todayStats.AverageParsingCertainty = todayStats.TotalParsingCertainty / todayStats.DetailsParsed;

                    statisticsRepository.Update(todayStats);
                    invoiceRepository.UpdateRange(entities);

                    await unitOfWork.Save(cancellationToken);

                    return "BatchAnalyzingCompleted";
                }
            });
        }
    }

}
