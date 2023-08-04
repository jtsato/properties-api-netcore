using Core.Commons;
using FluentValidation;

namespace Core.Domains.Properties.Query;

internal sealed class SearchPropertiesQueryValidator : AbstractValidator<SearchPropertiesQuery>
{
    public SearchPropertiesQueryValidator()
    {
        RuleFor(query => query.CreatedAt.From)
            .Must(ArgumentChecker.BeEmptyOrAValidDate)
            .WithMessage("ValidationPropertyFromCreatedAtIsInvalid");
        
        RuleFor(query => query.CreatedAt.To)
            .Must(ArgumentChecker.BeEmptyOrAValidDate)
            .WithMessage("ValidationPropertyToCreatedAtIsInvalid");
        
        RuleFor(query => query.UpdatedAt.From)
            .Must(ArgumentChecker.BeEmptyOrAValidDate)
            .WithMessage("ValidationPropertyFromUpdatedAtIsInvalid");
        
        RuleFor(query => query.UpdatedAt.To)
            .Must(ArgumentChecker.BeEmptyOrAValidDate)
            .WithMessage("ValidationPropertyToUpdatedAtIsInvalid");
    }
}