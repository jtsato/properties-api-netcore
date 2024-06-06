using FluentValidation;

namespace Core.Domains.Properties.Query;

internal sealed class GetPropertyByUuidQueryValidator : AbstractValidator<GetPropertyByUuidQuery>
{
    public GetPropertyByUuidQueryValidator()
    {
        RuleFor(query => query.Uuid)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("ValidationPropertyIdIsNullOrEmpty");
    }
}