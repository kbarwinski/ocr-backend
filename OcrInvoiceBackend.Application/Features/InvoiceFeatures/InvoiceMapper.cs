using AutoMapper;
using Newtonsoft.Json;
using OcrInvoiceBackend.Application.Common.Models;
using OcrInvoiceBackend.Application.Features.InvoiceFeatures.Models;
using OcrInvoiceBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures
{
    public sealed class InvoiceMapper : Profile
    {
        public InvoiceMapper()
        {
            CreateMap<Invoice, InvoiceDto>();
            CreateMap<Invoice, FullInvoiceDto>();
            CreateMap<InvoiceUpdateModel, Invoice>();

            CreateMap<Detail, DetailDto>();
        }
    }
}