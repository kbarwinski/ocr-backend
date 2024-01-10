using Microsoft.AspNetCore.Mvc;
using MediatR;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.UploadInvoices;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Queries.GetInvoice;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.ScanInvoice;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.DeleteInvoice;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.DeleteInvoices;
using OcrInvoiceBackend.Application.Common.Models;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.AnalyzeInvoice;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.UpdateInvoice;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.ScanInvoices;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.AnalyzeInvoices;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.UpdateInvoiceDetails;
using OcrInvoiceBackend.Domain.Entities;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Queries.GetInvoices;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.GenerateRandomInvoicePdf;

namespace OcrInvoiceBackend.API.Controllers
{
    [ApiController]
    [Route("invoices")]
    public class InvoiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<InvoiceDto>>> GetAll([FromQuery] GetInvoicesQuery query, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<InvoiceDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var request = new GetInvoiceQuery(id);

            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> UploadInvoices([FromForm] UploadInvoicesCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }


        [HttpPost]
        [Route("{id}/scan")]
        public async Task<ActionResult<FullInvoiceDto>> ScanInvoice(string id, CancellationToken cancellationToken)
        {
            var request = new ScanInvoiceCommand(id);

            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        [Route("batchscan")]
        public async Task<ActionResult> ScanInvoices([FromBody] List<string> ids, CancellationToken cancellationToken)
        {
            await _mediator.Send(new ScanInvoicesCommand(ids), cancellationToken);
            return NoContent();
        }

        [HttpPost]
        [Route("{id}/analyze")]
        public async Task<ActionResult<FullInvoiceDto>> AnalyzeInvoice(string id, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new AnalyzeInvoiceCommand(id), cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        [Route("batchanalyze")]
        public async Task<ActionResult> AnalyzeInvoices([FromBody] List<string> ids, CancellationToken cancellationToken)
        {
            await _mediator.Send(new AnalyzeInvoicesCommand(ids), cancellationToken);
            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<FullInvoiceDto>> UpdateInvoice(string id, [FromBody] InvoiceUpdateModel model, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new UpdateInvoiceCommand(id, model), cancellationToken);
            return Ok(response);
        }

        [HttpPut]
        [Route("{id}/details")]
        public async Task<ActionResult<FullInvoiceDto>> UpdateInvoiceDetails(string id, [FromBody] List<Detail> details, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new UpdateInvoiceDetailsCommand(id, details), cancellationToken);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteInvoice(Guid id, CancellationToken cancellationToken)
        {
            var request = new DeleteInvoiceCommand(id);

            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteInvoices([FromBody] List<Guid> ids, CancellationToken cancellationToken)
        {
            var request = new DeleteInvoicesCommand(ids);

            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }

        [HttpPost]
        [Route("generate")]
        public async Task<ActionResult<byte[]>> GenerateRandomInvoicePdf(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GenerateRandomInvoicePdfCommand(), cancellationToken);

            return File(response, "application/pdf");
        }
    }
}
