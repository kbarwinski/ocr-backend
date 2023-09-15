using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;
using OcrInvoiceBackend.Application.Repositories;
using OcrInvoiceBackend.Application.Services.TextRecognition;
using System.Text.Json.Serialization;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.ScanInvoice
{
    public class ScanInvoiceHandler : IRequestHandler<ScanInvoiceCommand, FullInvoiceDto>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly ITextRecognitionService _textRecognitionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScanInvoiceHandler(IInvoiceRepository invoiceRepository,
            IStatisticsRepository statisticsRepository,
            ITextRecognitionService textRecognitionService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _statisticsRepository = statisticsRepository;
            _textRecognitionService = textRecognitionService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FullInvoiceDto> Handle(ScanInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _invoiceRepository.Get(x => x.Id == new Guid(request.InvoiceId), cancellationToken, x => x.Scan);

            var startTime = DateTime.Now;
            var scanResults = _textRecognitionService.ScanInvoice(entity.FileData);
            var endTime = DateTime.Now;

            entity.Scan = scanResults;
            entity.IsScanned = true;

            var stats = await _statisticsRepository.GetTodayStatistics();

            stats.InvoicesScanned += 1;

            stats.TotalScanTime += (endTime - startTime).TotalSeconds;
            stats.AverageScanTime = stats.TotalScanTime / stats.InvoicesScanned;

            stats.TotalScanCertainty += entity.Scan.Certainty;
            stats.AverageScanCertainty = stats.TotalScanCertainty / stats.InvoicesScanned;

            _statisticsRepository.Update(stats);
            _invoiceRepository.Update(entity);

            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<FullInvoiceDto>(entity);
        }
    }
}
