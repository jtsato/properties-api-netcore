using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Core.Commons;
using Core.Commons.Paging;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;
using Core.Domains.Properties.UseCases;
using EntryPoint.WebApi.Commons;
using EntryPoint.WebApi.Commons.Controllers;
using EntryPoint.WebApi.Commons.Models;
using EntryPoint.WebApi.Domains.Commons;
using EntryPoint.WebApi.Domains.Properties.Models;
using EntryPoint.WebApi.Domains.Properties.Presenters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EntryPoint.WebApi.Domains.Properties.EntryPoints;

public sealed class SearchPropertiesController : ISearchPropertiesController
{
    private static readonly string[] SortableFields =
    [
        "id", // Identifier 
        "type", // Type: APARTMENT, HOUSE, ETC...
        "transaction", // Transaction: RENT or SALE
        "numberOfBedrooms", "numberOfToilets", "numberOfGarages", // Amenities
        "area", "builtArea", // Area
        "city", "state", "district", // Location
        "ranking", // Advertise
        "sellingPrice", "rentalTotalPrice", "rentalPrice", "priceByM2", // Prices 
        "createdAt", "updatedAt" // Dates
    ];

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISearchPropertiesUseCase _useCase;

    private const int DefaultMaxArea = 999999;
    private const byte DefaultMaxRooms = 255;
    private const float DefaultMaxPrice = 100000000;

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
        List<string> types = ResolveTypes(request);

        Transaction transaction = Transaction.GetByName(request.Transaction).OrElse(Transaction.All);
        string status = PropertyStatus.GetByName(request.Status).OrElse(PropertyStatus.All).Name;
        byte ranking = request.Ranking > 0 ? request.Ranking : (byte) 0;

        SearchPropertiesQueryBuilder builder = new SearchPropertiesQueryBuilder();

        bool isSale = transaction.Is(Transaction.All) || transaction.Is(Transaction.Sale);
        bool isRent = transaction.Is(Transaction.All) || transaction.Is(Transaction.Rent);

        (float sellingPriceMin, float sellingPriceMax) = ResolvePriceRange(isSale, request.MinPrice, request.MaxPrice);
        (float rentalTotalPriceMin, float rentalTotalPriceMax) = ResolvePriceRange(isRent, request.MinPrice, request.MaxPrice);

        (byte minBedrooms, byte maxBedrooms) = ResolveRoomRange(request.MinBedrooms, request.MaxBedrooms);
        (byte minToilets, byte maxToilets) = ResolveRoomRange(request.MinToilets, request.MaxToilets);
        (byte minGarages, byte maxGarages) = ResolveRoomRange(request.MinGarages, request.MaxGarages);

        (int minArea, int maxArea) = ResolveAreaRange(request.MinArea, request.MaxArea);
        (int minBuiltArea, int maxBuiltArea) = ResolveAreaRange(request.MinBuiltArea, request.MaxBuiltArea);

        List<string> districts = request.Districts?
            .Where(element => !string.IsNullOrEmpty(element))
            .SelectMany(element => element.Split(','))
            .ToList();

        builder
            .WithTypes(types)
            .WithTransaction(transaction.Name)
            .WithState(request.State)
            .WithCity(request.City)
            .WithDistricts(districts)
            .WithMinBedrooms(minBedrooms)
            .WithMaxBedrooms(maxBedrooms)
            .WithMinToilets(minToilets)
            .WithMaxToilets(maxToilets)
            .WithMinGarages(minGarages)
            .WithMaxGarages(maxGarages)
            .WithFromArea(minArea)
            .WithToArea(maxArea)
            .WithMinBuiltArea(minBuiltArea)
            .WithMaxBuiltArea(maxBuiltArea)
            .WithMinSellingPrice(sellingPriceMin)
            .WithToSellingPrice(sellingPriceMax)
            .WithFromRentalTotalPrice(rentalTotalPriceMin)
            .WithToRentalTotalPrice(rentalTotalPriceMax)
            .WithStatus(status)
            .WithRanking(ranking);

        return builder.Build();
    }

    private static List<string> ResolveTypes(SearchPropertiesRequest request)
    {
        return request.Types == null || request.Types.Count == 0 ? [] : request.Types;
    }

    private static (float Min, float Max) ResolvePriceRange(bool active, float minPrice, float maxPrice)
    {
        float min = active ? minPrice : 0;
        float max = active && maxPrice > 0 ? maxPrice : DefaultMaxPrice;

        return (min, max);
    }

    private static (byte Min, byte Max) ResolveRoomRange(byte minValue, byte maxValue)
    {
        byte min = minValue > 0 ? minValue : (byte) 0;
        byte max = maxValue > 0 ? maxValue : DefaultMaxRooms;

        return (min, max);
    }

    private static (int Min, int Max) ResolveAreaRange(int minValue, int maxValue)
    {
        int min = minValue > 0 ? minValue : 0;
        int max = maxValue > 0 ? maxValue : DefaultMaxArea;

        return (min, max);
    }
}