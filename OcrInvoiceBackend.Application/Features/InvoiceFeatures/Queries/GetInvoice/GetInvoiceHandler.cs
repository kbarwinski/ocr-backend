using AutoMapper;
using MediatR;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;
using OcrInvoiceBackend.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Queries.GetInvoice
{
    public class GetInvoiceHandler : IRequestHandler<GetInvoiceQuery, FullInvoiceDto>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;

        public GetInvoiceHandler(IInvoiceRepository invoiceRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
        }

        public async Task<FullInvoiceDto> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var entity = await _invoiceRepository.Get(x => x.Id == request.Id, cancellationToken, x => x.Details);
            var mapped = _mapper.Map<FullInvoiceDto>(entity);

            mapped.IsScanned = await _invoiceRepository.HasScanResults(entity.Id, cancellationToken);

            return mapped;
        }
    }

}
