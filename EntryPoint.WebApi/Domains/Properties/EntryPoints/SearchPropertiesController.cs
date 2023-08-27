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
    private static readonly string[] SortableFields = {"sellingPrice", "rentalTotalPrice", "rentalPrice", "priceByM2", "createdAt", "updatedAt"};

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
        string propertyType = PropertyType.GetByName(request.Type).OrElse(PropertyType.All).Name;

        SearchPropertiesQueryAdvertise advertise = new SearchPropertiesQueryAdvertise(request.Transaction);

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
            state: request.State,
            city: request.City,
            districts: request.Districts
        );

        SearchPropertiesQueryPrices prices = new SearchPropertiesQueryPrices
        {
            SellingPrice = Range<double>.Of(request.SellingPriceMin, request.SellingPriceMax),
            RentalTotalPrice = Range<double>.Of(request.RentalTotalPriceMin, request.RentalTotalPriceMax),
            PriceByM2 = Range<double>.Of(request.PriceByM2Min, request.PriceByM2Max)
        };

        SearchPropertiesQueryBuilder builder = new SearchPropertiesQueryBuilder();

        builder
            .WithType(propertyType)
            .WithTransaction(advertise.Transaction)
            .WithState(location.State)
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
            .WithFromPriceByM2(prices.PriceByM2.From)
            .WithToPriceByM2(prices.PriceByM2.To)
            .WithStatus(request.Status);

        return builder.Build();
    }
}