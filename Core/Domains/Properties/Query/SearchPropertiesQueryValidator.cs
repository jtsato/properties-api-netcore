using FluentValidation;

namespace Core.Domains.Properties.Query;

internal sealed class SearchPropertiesQueryValidator : AbstractValidator<SearchPropertiesQuery>
{
    public SearchPropertiesQueryValidator()
    {
        RuleFor(query => query.Advertise.Transaction)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("ValidationPropertyTransactionIsNullOrEmpty")
            .NotEmpty()
            .WithMessage("ValidationPropertyTransactionIsNullOrEmpty");
    }
}