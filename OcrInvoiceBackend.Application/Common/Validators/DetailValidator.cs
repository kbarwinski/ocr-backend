using FluentValidation;
using FluentValidation.Validators;
using OcrInvoiceBackend.Application.Repositories;
using OcrInvoiceBackend.Domain.Entities;

namespace OcrInvoiceBackend.Application.Common.Validators
{
    public class DetailValidator : AbstractValidator<Detail>
    {
        private readonly IParsingFieldRepository _parsingFieldRepository;

        public DetailValidator(IParsingFieldRepository parsingFieldRepository)
        {
            _parsingFieldRepository = parsingFieldRepository;

            RuleFor(detail => detail)
                .MustAsync(IsValidDetailAsync)
                .WithMessage(x => $"Invalid detail {x.Name}");
        }

        private async Task<bool> IsValidDetailAsync(Detail detail, CancellationToken cancellationToken)
        {
            var correspondingField = (await _parsingFieldRepository
                .GetByCondition(x => x.Name == detail.Name, cancellationToken))
                .FirstOrDefault();

            if (correspondingField == null)
                return false;

            return correspondingField.ValidateDetail(detail);
        }
    }
}
