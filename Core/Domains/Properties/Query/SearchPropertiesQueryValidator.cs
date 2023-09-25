﻿using Core.Commons;
using Core.Domains.Properties.Models;
using FluentValidation;

namespace Core.Domains.Properties.Query;

internal sealed class SearchPropertiesQueryValidator : AbstractValidator<SearchPropertiesQuery>
{
    public SearchPropertiesQueryValidator()
    {
        RuleFor(query => query.Type)
            .Cascade(CascadeMode.Stop)
            .Must(ArgumentChecker.IsValidEnumOf<PropertyType>)
            .WithMessage("ValidationPropertyTypeIsInvalid");

        RuleFor(query => query.Advertise.Transaction)
            .Cascade(CascadeMode.Stop)
            .Must(ArgumentChecker.IsValidEnumOf<Transaction>)
            .WithMessage("ValidationPropertyTransactionIsInvalid");

        RuleFor(query => query.Attributes.NumberOfBedrooms)
            .Cascade(CascadeMode.Stop)
            .Must(numberOfBedrooms => numberOfBedrooms.From <= numberOfBedrooms.To || numberOfBedrooms.To == 0)
            .WithMessage("ValidationPropertyNumberOfBedroomsIsInvalid");

        RuleFor(query => query.Attributes.NumberOfToilets)
            .Cascade(CascadeMode.Stop)
            .Must(numberOfToilets => numberOfToilets.From <= numberOfToilets.To || numberOfToilets.To == 0)
            .WithMessage("ValidationPropertyNumberOfToiletsIsInvalid");

        RuleFor(query => query.Attributes.NumberOfGarages)
            .Cascade(CascadeMode.Stop)
            .Must(numberOfGarages => numberOfGarages.From <= numberOfGarages.To || numberOfGarages.To == 0)
            .WithMessage("ValidationPropertyNumberOfGaragesIsInvalid");

        RuleFor(query => query.Attributes.Area)
            .Cascade(CascadeMode.Stop)
            .Must(area => area.From <= area.To || area.To == 0)
            .WithMessage("ValidationPropertyAreaIsInvalid");

        RuleFor(query => query.Attributes.BuiltArea)
            .Cascade(CascadeMode.Stop)
            .Must(builtArea => builtArea.From <= builtArea.To || builtArea.To == 0)
            .WithMessage("ValidationPropertyBuiltAreaIsInvalid");

        RuleFor(query => query.Prices.SellingPrice)
            .Cascade(CascadeMode.Stop)
            .Must(sellingPrice => sellingPrice.From <= sellingPrice.To || sellingPrice.To == 0)
            .WithMessage("ValidationPropertySellingPriceIsInvalid");
        
        RuleFor(query => query.Prices.RentalTotalPrice)
            .Cascade(CascadeMode.Stop)
            .Must(rentalTotalPrice => rentalTotalPrice.From <= rentalTotalPrice.To || rentalTotalPrice.To == 0)
            .WithMessage("ValidationPropertyRentalTotalPriceIsInvalid");
        
        RuleFor(query => query.Prices.RentalPrice)
            .Cascade(CascadeMode.Stop)
            .Must(rentalPrice => rentalPrice.From <= rentalPrice.To || rentalPrice.To == 0)
            .WithMessage("ValidationPropertyRentalPriceIsInvalid");
        
        RuleFor(query => query.Prices.PriceByM2)
            .Cascade(CascadeMode.Stop)
            .Must(priceByM2 => priceByM2.From <= priceByM2.To || priceByM2.To == 0)
            .WithMessage("ValidationPropertyPriceByM2IsInvalid");
        
        RuleFor(query => query.Status)
            .Cascade(CascadeMode.Stop)
            .Must(ArgumentChecker.IsValidEnumOf<PropertyStatus>)
            .WithMessage("ValidationPropertyStatusIsInvalid");
    }
}