using MediatR;
using OcrInvoiceBackend.Application.Common.Exceptions;
using OcrInvoiceBackend.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.DeleteInvoices
{
    public class DeleteInvoicesHandler : IRequestHandler<DeleteInvoicesCommand>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInvoicesHandler(IInvoiceRepository invoiceRepository, IUnitOfWork unitOfWork)
        {
            _invoiceRepository = invoiceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteInvoicesCommand request, CancellationToken cancellationToken)
        {
            var entities = await _invoiceRepository.GetByIds(request.Ids, cancellationToken);

            if (entities.Count != request.Ids.Count)
                throw new NotFoundException("Not all invoices had been found!");

            _invoiceRepository.DeleteRange(entities);
            await _unitOfWork.Save(cancellationToken);
        }
    }
}
