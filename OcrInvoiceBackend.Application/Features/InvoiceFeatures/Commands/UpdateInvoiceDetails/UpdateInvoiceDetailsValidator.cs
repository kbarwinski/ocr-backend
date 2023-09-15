using FluentValidation;
using OcrInvoiceBackend.Application.Common.Validators;
using OcrInvoiceBackend.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.InvoiceFeatures.Commands.UpdateInvoiceDetails
{
    public class UpdateInvoiceDetailsValidator : AbstractValidator<UpdateInvoiceDetailsCommand>
    {
        public UpdateInvoiceDetailsValidator(IParsingFieldRepository parsingFieldRepository) 
        {
            RuleFor(x => x.Details)
                .ForEach(x => x.SetValidator(new DetailValidator(parsingFieldRepository)));
        }
    }
}
