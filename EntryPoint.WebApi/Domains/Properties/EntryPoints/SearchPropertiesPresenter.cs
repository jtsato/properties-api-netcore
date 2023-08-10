using System;
using System.Collections.Generic;
using System.Globalization;
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
            Transaction = property.Advertise.Transaction.Name,
            Title = property.Advertise.Title,
            Description = property.Advertise.Description,
            Url = property.Advertise.Url,
            RefId = property.Advertise.RefId,
            Images = property.Advertise.Images,
            NumberOfBedrooms = property.Attributes.NumberOfBedrooms,
            NumberOfToilets = property.Attributes.NumberOfToilets,
            NumberOfGarages = property.Attributes.NumberOfGarages,
            Area = property.Attributes.Area,
            BuiltArea = property.Attributes.BuiltArea,
            City = property.Location.City,
            District = property.Location.District,
            Address = property.Location.Address,
            SellingPrice = property.Prices.SellingPrice,
            RentalTotalPrice = property.Prices.RentalTotalPrice,
            RentalPrice = property.Prices.RentalPrice,
            Discount = property.Prices.Discount,
            CondominiumFee = property.Prices.CondominiumFee,
            PriceByM2 = property.Prices.PriceByM2,
            CreatedAt = property.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
            UpdatedAt = property.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
            Href = $"{baseUrl.AppendIfMissing("/")}{property.Id}"
        };
    }
}