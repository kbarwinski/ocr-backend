using MediatR;
using Microsoft.AspNetCore.Mvc;
using OcrInvoiceBackend.Application.Features.StatisticsFeatures.Models;
using OcrInvoiceBackend.Application.Features.StatisticsFeatures.Queries.GetStatistics;
using OcrInvoiceBackend.Application.Features.StatisticsFeatures.Queries.GetTotalStatistics;

namespace OcrStatisticsBackend.API.Controllers
{
    [ApiController]
    [Route("statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatisticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<StatisticsDto>>> GetList([FromQuery] GetStatisticsQuery query, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpGet("total")]
        public async Task<ActionResult<StatisticsDto>> Get([FromQuery] GetTotalStatisticsQuery query, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(query, cancellationToken);
            return Ok(response);
        }
    }
}
