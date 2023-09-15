using AutoMapper;
using MediatR;
using OcrInvoiceBackend.Application.Features.StatisticsFeatures.Models;
using OcrInvoiceBackend.Application.Features.StatisticsFeatures.Queries.GetStatistics;
using OcrInvoiceBackend.Application.Repositories;
using OcrInvoiceBackend.Domain.Entities;
using OcrStatisticsBackend.Application.Features.StatisticsFeatures.Queries.GetStatistics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OcrStatisticsBackend.Application.Features.StatisticsFeatures.Queries.GetStatistics
{
    public sealed class GetStatisticsHandler : IRequestHandler<GetStatisticsQuery, GetStatisticsResponse>
    {
        private readonly IStatisticsRepository _invoiceRepository;
        private readonly IMapper _mapper;

        public GetStatisticsHandler(IStatisticsRepository invoiceRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
        }

        public async Task<GetStatisticsResponse> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
        {
            var filterRules = new Dictionary<Expression<Func<Statistics, bool>>, bool>
            {
                { x => x.DateCreated.Date >= request.Start.Value.Date && x.DateCreated.Date <= request.End.Value.Date, request.Start.HasValue && request.End.HasValue }
            };

            var orderRules = new Dictionary<string, Expression<Func<Statistics, object>>>
            {
                ["id"] = x => x.Id,
                ["dateCreated"] = x => x.DateCreated,
            };

            var (res, pagination) = await _invoiceRepository
                .GetFilteredAndOrderedPage(request, filterRules, orderRules, cancellationToken);

            return new GetStatisticsResponse()
            {
                Result = _mapper.Map<List<StatisticsDto>>(res),
                Pagination = pagination
            };
        }
    }
}
