using MediatR;
using OcrInvoiceBackend.Application.Services.Automation;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.GenerateRandomInvoicePdf
{
    internal class GenerateRandomInvoicePdfHandler : IRequestHandler<GenerateRandomInvoicePdfCommand, byte[]>
    {
        private readonly IInvoiceGeneratorService _invoiceGenerator;

        public GenerateRandomInvoicePdfHandler(IInvoiceGeneratorService invoiceGenerator)
        {
            _invoiceGenerator = invoiceGenerator;
        }

        public async Task<byte[]> Handle(GenerateRandomInvoicePdfCommand request, CancellationToken cancellationToken)
        {
            return await _invoiceGenerator.GenerateRandomInvoicePdf();
        }
    }
}
