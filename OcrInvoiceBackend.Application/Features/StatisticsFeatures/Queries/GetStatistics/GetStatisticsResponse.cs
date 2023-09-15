using OcrInvoiceBackend.Application.Common.Models;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;
using OcrInvoiceBackend.Application.Features.StatisticsFeatures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.StatisticsFeatures.Queries.GetStatistics
{
    public sealed record GetStatisticsResponse()
    {
        public List<StatisticsDto> Result { get; set; }
        public PaginationResponse Pagination { get; set; }
    }
}
