using AutoMapper;
using MediatR;
using OcrInvoiceBackend.Application.Common.Exceptions;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.DeleteInvoice;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;
using OcrInvoiceBackend.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.UpdateInvoiceDetails
{
    public class UpdateInvoiceDetailsHandler : IRequestHandler<UpdateInvoiceDetailsCommand, FullInvoiceDto>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateInvoiceDetailsHandler(IInvoiceRepository invoiceRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FullInvoiceDto> Handle(UpdateInvoiceDetailsCommand request, CancellationToken cancellationToken)
        {
            var entity = await _invoiceRepository.Get(x => x.Id == new Guid(request.Id), cancellationToken, x => x.Details);

            if (entity == null)
                throw new NotFoundException("Invoice not found");

            entity.Details = request.Details;
            entity.IsParsed = true;
            _invoiceRepository.Update(entity);

            await _unitOfWork.Save(cancellationToken);

            var mapped = _mapper.Map<FullInvoiceDto>(entity);

            return mapped;
        }
    }
}
