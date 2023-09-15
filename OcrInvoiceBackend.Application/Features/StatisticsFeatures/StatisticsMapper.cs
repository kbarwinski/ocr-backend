using AutoMapper;
using OcrInvoiceBackend.Application.Features.StatisticsFeatures.Models;
using OcrInvoiceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrStatisticsBackend.Application.Features.StatisticsFeatures
{
    public sealed class StatisticsMapper : Profile
    {
        public StatisticsMapper()
        {
            CreateMap<Statistics, StatisticsDto>();
        }
    }
}
