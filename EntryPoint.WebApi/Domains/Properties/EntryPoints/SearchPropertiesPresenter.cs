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
            Area = Convert.ToString(property.Attributes.Area),
            BuiltArea = Convert.ToString(property.Attributes.BuiltArea),
            City = property.Location.City,
            District = property.Location.District,
            Address = property.Location.Address,
            SellingPrice = Convert.ToString(property.Prices.SellingPrice, CultureInfo.InvariantCulture),
            RentalTotalPrice = Convert.ToString(property.Prices.RentalTotalPrice, CultureInfo.InvariantCulture),
            RentalPrice = Convert.ToString(property.Prices.RentalPrice, CultureInfo.InvariantCulture),
            Discount = Convert.ToString(property.Prices.Discount, CultureInfo.InvariantCulture),
            CondominiumFee = Convert.ToString(property.Prices.CondominiumFee, CultureInfo.InvariantCulture),
            PriceByM2 = Convert.ToString(property.Prices.PriceByM2, CultureInfo.InvariantCulture),
            CreatedAt = property.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
            UpdatedAt = property.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
            Href = $"{baseUrl.AppendIfMissing("/")}{property.Id}"
        };
    }
}