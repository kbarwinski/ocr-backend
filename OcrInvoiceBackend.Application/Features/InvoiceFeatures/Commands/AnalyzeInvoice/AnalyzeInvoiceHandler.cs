using AutoMapper;
using MediatR;
using OcrInvoiceBackend.Application.Common.Exceptions;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;
using OcrInvoiceBackend.Application.Repositories;
using OcrInvoiceBackend.Domain.Entities;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.AnalyzeInvoice
{
    public class AnalyzeInvoiceHandler : IRequestHandler<AnalyzeInvoiceCommand, List<DetailDto>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IParsingFieldRepository _parsingFieldRepository;
        private readonly IStatisticsRepository _statisticsRepository;

        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public AnalyzeInvoiceHandler(
            IInvoiceRepository invoiceRepository,
            IParsingFieldRepository parsingFieldRepository,
            IStatisticsRepository statisticsRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _invoiceRepository = invoiceRepository;
            _parsingFieldRepository = parsingFieldRepository;
            _statisticsRepository = statisticsRepository;

            _mapper = mapper;

            _unitOfWork = unitOfWork;
        }

        public async Task<List<DetailDto>> Handle(AnalyzeInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _invoiceRepository.Get(x => x.Id == new Guid(request.Id), cancellationToken, x => x.Scan, x => x.Details);
            if (entity == null)
                throw new NotFoundException("Invoice not found.");

            var availableFields = await _parsingFieldRepository
                .GetByCondition(x => true, cancellationToken);

            var detailTasks = availableFields.Select(detail => detail.ParseForField(entity.Scan)).ToList();

            var todayStats = await _statisticsRepository.GetTodayStatistics();
            var parsingStartTime = DateTime.Now;

            var results = (await Task.WhenAll(detailTasks)).ToList();

            foreach (var field in availableFields)
                field.DeduceFromResults(results);

            var parsingEndTime = DateTime.Now;

            todayStats.InvoicesParsed += 1;
            todayStats.DetailsParsed += results.Count;

            todayStats.TotalParsingTime += (parsingEndTime - parsingStartTime).TotalSeconds;
            todayStats.AverageParsingTime = todayStats.TotalParsingTime / todayStats.InvoicesParsed;

            todayStats.TotalParsingCertainty += results.Sum(result => result.Certainty);
            todayStats.AverageParsingCertainty = todayStats.TotalParsingCertainty / todayStats.DetailsParsed;

            if (entity.Details.Count <= 0)
            {
                entity.Details = results;
                _invoiceRepository.Update(entity);
            }

            _statisticsRepository.Update(todayStats);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<List<DetailDto>>(results);
        }
    }
}
