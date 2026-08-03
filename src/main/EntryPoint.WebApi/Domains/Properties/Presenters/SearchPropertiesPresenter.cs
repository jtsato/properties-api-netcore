using System.Collections.Generic;
using System.Linq;
using Core.Commons.Paging;
using Core.Domains.Properties.Models;
using EntryPoint.WebApi.Domains.Properties.Models;

namespace EntryPoint.WebApi.Domains.Properties.Presenters;

public static class SearchPropertiesPresenter
{
    public static PageableSearchPropertiesResponse Of(Page<Property> page, string baseUrl)
    {
        List<SearchPropertiesInnerResponse> content = [.. page.Content.Select(Of)];
        return new PageableSearchPropertiesResponse(content, page.Pageable);
    }

    private static SearchPropertiesInnerResponse Of(Property property)
    {
        return new SearchPropertiesInnerResponse
        {
            Id = property.Id,
            Uuid = property.Uuid,
            Transaction = property.Advertise.Transaction.Name.ToUpperInvariant(),
            Type = property.Type.Name.ToUpperInvariant(),
            CoverImage = property.Advertise.Images.FirstOrDefault(),
            NumberOfBedrooms = property.Attributes.NumberOfBedrooms,
            NumberOfGarages = property.Attributes.NumberOfGarages,
            Area = property.Attributes.Area,
            BuiltArea = property.Attributes.BuiltArea,
            State = property.Location.State,
            City = property.Location.City,
            District = property.Location.District,
            SellingPrice = property.Prices.SellingPrice,
            RentalTotalPrice = property.Prices.RentalTotalPrice,
            Ranking = property.Ranking
        };
    }
}
