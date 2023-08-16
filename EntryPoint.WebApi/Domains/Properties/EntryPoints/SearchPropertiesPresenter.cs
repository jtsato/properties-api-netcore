﻿using System.Collections.Generic;
using System.Linq;
using Core.Commons.Extensions;
using Core.Commons.Paging;
using Core.Domains.Properties.Models;
using EntryPoint.WebApi.Domains.Properties.Models;

namespace EntryPoint.WebApi.Domains.Properties.EntryPoints;

public static class SearchPropertiesPresenter
{
    public static PageableSearchPropertiesResponse Of(Page<Property> page, string baseUrl)
    {
        List<SearchPropertiesInnerResponse> content = page.Content.Select(it => Of(it, baseUrl)).ToList();
        return new PageableSearchPropertiesResponse(content, page.Pageable);
    }

    private static SearchPropertiesInnerResponse Of(Property property, string baseUrl)
    {
        return new SearchPropertiesInnerResponse
        {
            Id = property.Id,
            TenantId = property.Advertise.TenantId,
            Transaction = property.Advertise.Transaction.Name.ToUpperInvariant(),
            Title = property.Advertise.Title,
            // TODO: Replace the full description with a short description.
            Description = property.Advertise.Description,
            Url = property.Advertise.Url,
            RefId = property.Advertise.RefId,
            Images = property.Advertise.Images,
            NumberOfBedrooms = property.Attributes.NumberOfBedrooms,
            NumberOfToilets = property.Attributes.NumberOfToilets,
            NumberOfGarages = property.Attributes.NumberOfGarages,
            Area = property.Attributes.Area,
            BuiltArea = property.Attributes.BuiltArea,
            State = property.Location.State,
            City = property.Location.City,
            District = property.Location.District,
            Address = property.Location.Address,
            SellingPrice = property.Prices.SellingPrice,
            RentalTotalPrice = property.Prices.RentalTotalPrice,
            RentalPrice = property.Prices.RentalPrice,
            Discount = property.Prices.Discount,
            CondominiumFee = property.Prices.CondominiumFee,
            PriceByM2 = property.Prices.PriceByM2,
            Ranking = property.Ranking,
            Status = property.Status.Name.ToUpperInvariant(),
            // TODO: Double check if the date time zone is correct.
            CreatedAt = property.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
            UpdatedAt = property.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
            Href = $"{baseUrl.AppendIfMissing("/")}{property.Id}"
        };
    }
}