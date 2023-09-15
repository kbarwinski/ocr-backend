using MediatR;
using AutoMapper;
using OcrInvoiceBackend.Application.Repositories;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;
using System.Linq.Expressions;
using OcrInvoiceBackend.Domain.Entities;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Queries.GetInvoices
{
    public sealed class GetInvoicesHandler : IRequestHandler<GetInvoicesQuery, GetInvoicesResponse>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;

        public GetInvoicesHandler(IInvoiceRepository invoiceRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
        }

        public async Task<GetInvoicesResponse> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
        {
            var filterRules = new Dictionary<Expression<Func<Invoice, bool>>, bool>
            {
                { x => x.DateCreated >= request.Start.Value && x.DateCreated <= request.End.Value, request.Start.HasValue && request.End.HasValue },
                { x => x.IsScanned == request.IsScanned.Value, request.IsScanned.HasValue },
                { x => x.IsParsed == request.IsParsed.Value, request.IsParsed.HasValue },
                { x => x.IsApproved == request.IsApproved.Value, request.IsApproved.HasValue }
            };

            var orderRules = new Dictionary<string, Expression<Func<Invoice, object>>>
            {
                ["id"] = x => x.Id,
                ["name"] = x => x.Name,
                ["certainty"] = x => x.Scan.Certainty == null ? 0.0f : x.Scan.Certainty,
            };

            var (res, pagination) = await _invoiceRepository
                .GetFilteredAndOrderedPage(request, filterRules, orderRules, cancellationToken);

            return new GetInvoicesResponse()
            {
                Result = _mapper.Map<List<InvoiceDto>>(res),
                Pagination = pagination
            };
        }
    }
}
