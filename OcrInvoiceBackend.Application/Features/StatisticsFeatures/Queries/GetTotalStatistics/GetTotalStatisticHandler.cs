using AutoMapper;
using MediatR;
using OcrInvoiceBackend.Application.Features.StatisticsFeatures.Models;
using OcrInvoiceBackend.Application.Repositories;
using OcrInvoiceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.StatisticsFeatures.Queries.GetTotalStatistics
{
    public sealed class GetTotalStatisticHandler : IRequestHandler<GetTotalStatisticsQuery, GetTotalStatisticsResponse>
    {
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly IMapper _mapper;

        public GetTotalStatisticHandler(IStatisticsRepository statisticsRepository, IMapper mapper)
        {
            _statisticsRepository = statisticsRepository;
            _mapper = mapper;
        }

        public async Task<GetTotalStatisticsResponse> Handle(GetTotalStatisticsQuery request, CancellationToken cancellationToken)
        {
            var startDate = request.Start ?? DateTime.MinValue.ToUniversalTime();
            var endDate = request.End ?? DateTime.UtcNow;

            var stats = await _statisticsRepository.GetWhere(s => s.DateCreated >= startDate && s.DateCreated <= endDate, cancellationToken);

            var result = new Statistics()
            {
                InvoicesUploaded = stats.Sum(s => s.InvoicesUploaded),
                TotalUploadTime = stats.Sum(s => s.TotalUploadTime),
                AverageUploadTime = stats.Average(s => s.AverageUploadTime),

                InvoicesScanned = stats.Sum(s => s.InvoicesScanned),
                TotalScanTime = stats.Sum(s => s.TotalScanTime),
                AverageScanTime = stats.Average(s => s.AverageScanTime),
                TotalScanCertainty = stats.Sum(s => s.TotalScanCertainty),
                AverageScanCertainty = stats.Average(s => s.AverageScanCertainty),

                InvoicesParsed = stats.Sum(s => s.InvoicesParsed),
                DetailsParsed = stats.Sum(s => s.DetailsParsed),
                TotalParsingTime = stats.Sum(s => s.TotalParsingTime),
                AverageParsingTime = stats.Average(s => s.AverageParsingTime),
                TotalParsingCertainty = stats.Sum(s => s.TotalParsingCertainty),
                AverageParsingCertainty = stats.Average(s => s.AverageParsingCertainty),

                InvoicesApproved = stats.Sum(s => s.InvoicesApproved),
                DetailsApproved = stats.Sum(s => s.DetailsApproved),
                DetailsGuessed = stats.Sum(s => s.DetailsGuessed),
                DetailsCorrected = stats.Sum(s => s.DetailsCorrected),
            };

            return new GetTotalStatisticsResponse
            {
                StartDate = stats.Min(x => x.DateCreated),
                EndDate = stats.Max(x => x.DateCreated),
                Result = _mapper.Map<StatisticsDto>(result)
            };
        }
    }

}
