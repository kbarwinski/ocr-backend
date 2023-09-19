using MediatR;
using OcrInvoiceBackend.Application.Common.Behaviors;
using OcrInvoiceBackend.Application.Common.Models;
using OcrInvoiceBackend.Application.Features.StatisticsFeatures.Queries.GetStatistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.StatisticsFeatures.Queries.GetTotalStatistics
{
    public sealed record GetTotalStatisticsQuery(DateTime? Start, DateTime? End) : IRequest<GetTotalStatisticsResponse>;
}
