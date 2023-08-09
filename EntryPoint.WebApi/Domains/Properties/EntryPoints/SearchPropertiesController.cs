using System.Net;
using System.Threading.Tasks;
using Core.Commons;
using Core.Commons.Models;
using Core.Commons.Paging;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;
using Core.Domains.Properties.UseCases;
using EntryPoint.WebApi.Commons;
using EntryPoint.WebApi.Commons.Controllers;
using EntryPoint.WebApi.Commons.Models;
using EntryPoint.WebApi.Domains.Commons;
using EntryPoint.WebApi.Domains.Properties.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EntryPoint.WebApi.Domains.Properties.EntryPoints;

public sealed class SearchPropertiesController : ISearchPropertiesController
{
    private static readonly string[] SortableFields = {"SellingPrice", "RentalTotalPrice", "RentalPrice", "PriceByM2", "CreatedDate", "UpdatedDate"};

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISearchPropertiesUseCase _useCase;

    public SearchPropertiesController(IHttpContextAccessor httpContextAccessor, ISearchPropertiesUseCase useCase)
    {
        _httpContextAccessor = ArgumentValidator.CheckNull(httpContextAccessor, nameof(httpContextAccessor));
        _useCase = ArgumentValidator.CheckNull(useCase, nameof(useCase));
    }

    public async Task<IActionResult> ExecuteAsync(SearchPropertiesRequest request, QPageRequest qPageRequest)
    {
        SearchPropertiesQuery query = BuildSearchPropertiesQuery(request);

        string orderBy = OrderByHelper.Sanitize(SortableFields, qPageRequest.OrderBy);
        PageRequest pageRequest = PageRequestHelper.Of(qPageRequest.PageNumber, qPageRequest.PageSize, orderBy);

        Page<Property> page = await _useCase.ExecuteAsync(query, pageRequest);
        string baseUrl = UrlHelper.GetBaseUrl(_httpContextAccessor.HttpContext);

        PageableSearchPropertiesResponse response = SearchPropertiesPresenter.Of(page, baseUrl);

        if (page.Pageable.TotalPages == 0 || pageRequest.PageNumber >= page.Pageable.TotalPages)
        {
            return await ResponseBuilder.BuildResponse(HttpStatusCode.NoContent);
        }

        return await (page.Pageable.TotalPages switch
        {
            1 => ResponseBuilder.BuildResponse(HttpStatusCode.OK, new SearchPropertiesResponse(response.Content)),
            _ => ResponseBuilder.BuildResponse(HttpStatusCode.PartialContent, response)
        });
    }

    private static SearchPropertiesQuery BuildSearchPropertiesQuery(SearchPropertiesRequest request)
    {
        PropertyType propertyType = PropertyType.GetByName(request.Type).OrElse(PropertyType.All);
        
        SearchPropertiesQueryAdvertise advertise = new SearchPropertiesQueryAdvertise
        (
            request.Transaction,
            request.RefId
        );

        Range<byte> numberOfBedrooms = Range<byte>.Of(request.NumberOfBedroomsMin, request.NumberOfBedroomsMax);
        Range<byte> numberOfToilets = Range<byte>.Of(request.NumberOfToiletsMin, request.NumberOfToiletsMax);
        Range<byte> numberOfGarages = Range<byte>.Of(request.NumberOfGaragesMin, request.NumberOfGaragesMax);
        Range<int> area = Range<int>.Of(request.AreaMin, request.AreaMax);
        Range<int> builtArea = Range<int>.Of(request.BuiltAreaMin, request.BuiltAreaMax);

        SearchPropertiesQueryAttributes attributes = new SearchPropertiesQueryAttributes
        {
            NumberOfBedrooms = numberOfBedrooms,
            NumberOfToilets = numberOfToilets,
            NumberOfGarages = numberOfGarages,
            Area = area,
            BuiltArea = builtArea
        };

        SearchPropertiesQueryLocation location = new SearchPropertiesQueryLocation
        (
            city: request.City,
            districts: request.Districts
        );

        SearchPropertiesQueryPrices prices = new SearchPropertiesQueryPrices
        {
            SellingPrice = Range<decimal>.Of(request.SellingPriceMin, request.SellingPriceMax),
            RentalTotalPrice = Range<decimal>.Of(request.RentalTotalPriceMin, request.RentalTotalPriceMax),
            RentalPrice = Range<decimal>.Of(request.RentalPriceMin, request.RentalPriceMax),
            PriceByM2 = Range<decimal>.Of(request.PriceByM2Min, request.PriceByM2Max)
        };

        Range<string> createdAt = Range<string>.Of(request.FromCreatedAt, request.ToCreatedAt);
        Range<string> updatedAt = Range<string>.Of(request.FromUpdatedAt, request.ToUpdatedAt);

        SearchPropertiesQueryBuilder builder = new SearchPropertiesQueryBuilder();
        builder.WithType(propertyType.Name)
            .WithTransaction(advertise.Transaction)
            .WithRefId(advertise.RefId)
            .WithCity(location.City)
            .WithDistricts(location.Districts)
            .WithFromNumberOfBedrooms(attributes.NumberOfBedrooms.From)
            .WithToNumberOfBedrooms(attributes.NumberOfBedrooms.To)
            .WithFromNumberOfToilets(attributes.NumberOfToilets.From)
            .WithToNumberOfToilets(attributes.NumberOfToilets.To)
            .WithFromNumberOfGarages(attributes.NumberOfGarages.From)
            .WithToNumberOfGarages(attributes.NumberOfGarages.To)
            .WithFromArea(attributes.Area.From)
            .WithToArea(attributes.Area.To)
            .WithFromBuiltArea(attributes.BuiltArea.From)
            .WithToBuiltArea(attributes.BuiltArea.To)
            .WithFromSellingPrice(prices.SellingPrice.From)
            .WithToSellingPrice(prices.SellingPrice.To)
            .WithFromRentalTotalPrice(prices.RentalTotalPrice.From)
            .WithToRentalTotalPrice(prices.RentalTotalPrice.To)
            .WithFromRentalPrice(prices.RentalPrice.From)
            .WithToRentalPrice(prices.RentalPrice.To)
            .WithFromPriceByM2(prices.PriceByM2.From)
            .WithToPriceByM2(prices.PriceByM2.To)
            .WithFromCreatedAt(createdAt.From)
            .WithToCreatedAt(createdAt.To)
            .WithFromUpdatedAt(updatedAt.From)
            .WithToUpdatedAt(updatedAt.To);
        
        return builder.Build();
    }
}