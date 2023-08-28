using FluentValidation;

namespace Core.Domains.Properties.Query;

internal sealed class GetPropertyByIdQueryValidator : AbstractValidator<GetPropertyByIdQuery>
{
    public GetPropertyByIdQueryValidator()
    {
        RuleFor(query => query.Id)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("ValidationPropertyIdIsNullOrEmpty");
    }
}