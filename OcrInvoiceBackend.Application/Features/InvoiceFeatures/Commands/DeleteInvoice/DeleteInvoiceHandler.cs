using MediatR;
using OcrInvoiceBackend.Application.Common.Exceptions;
using OcrInvoiceBackend.Application.Repositories;


namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.DeleteInvoice
{
    public class DeleteInvoiceHandler : IRequestHandler<DeleteInvoiceCommand>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInvoiceHandler(IInvoiceRepository invoiceRepository, IUnitOfWork unitOfWork)
        {
            _invoiceRepository = invoiceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _invoiceRepository.Get(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
                throw new NotFoundException("Invoice not found");

            _invoiceRepository.Delete(entity);
            await _unitOfWork.Save(cancellationToken);
        }
    }
}
