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
    {
        "id", // Identifier 
        "type", // Type: APARTMENT, HOUSE, ETC...
        "transaction", // Transaction: RENT or SALE
        "numberOfBedrooms", "numberOfToilets", "numberOfGarages", // Amenities
        "area", "builtArea", // Area
        "city", "state", "district", // Location
        "ranking", // Advertise
        "sellingPrice", "rentalTotalPrice", "rentalPrice", "priceByM2", // Prices 
        "createdAt", "updatedAt" // Dates
    };

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
        List<string> types = request.Types == null || request.Types.Count == 0 ? new List<string>() : request.Types;

        Transaction transaction = Transaction.GetByName(request.Transaction).OrElse(Transaction.All);
        string status = PropertyStatus.GetByName(request.Status).OrElse(PropertyStatus.All).Name;
        byte ranking = request.Ranking > 0 ? request.Ranking : (byte) 0;

        SearchPropertiesQueryBuilder builder = new SearchPropertiesQueryBuilder();

        bool isSale = transaction.Is(Transaction.All) || transaction.Is(Transaction.Sale);
        bool isRent = transaction.Is(Transaction.All) || transaction.Is(Transaction.Rent);

        float sellingPriceMin = isSale ? request.MinPrice : 0;
        float sellingPriceMax = isSale && request.MaxPrice > 0 ? request.MaxPrice : DefaultMaxPrice;
        float rentalPriceMin = isRent ? request.MinPrice : 0;
        float rentalPriceMax = isRent && request.MaxPrice > 0 ? request.MaxPrice : DefaultMaxPrice;
        
        byte minBedrooms = request.MinBedrooms > 0 ? request.MinBedrooms : (byte)0;
        byte maxBedrooms = request.MaxBedrooms > 0 ? request.MaxBedrooms : DefaultMaxRooms;
        byte minToilets = request.MinToilets > 0 ? request.MinToilets : (byte)0;
        byte maxToilets = request.MaxToilets > 0 ? request.MaxToilets : DefaultMaxRooms;
        byte minGarages = request.MinGarages > 0 ? request.MinGarages : (byte)0;
        byte maxGarages = request.MaxGarages > 0 ? request.MaxGarages : DefaultMaxRooms;
        
        int minArea = request.MinArea > 0 ? request.MinArea : 0;
        int maxArea = request.MaxArea > 0 ? request.MaxArea : DefaultMaxArea;
        int minBuiltArea = request.MinBuiltArea > 0 ? request.MinBuiltArea : 0;
        int maxBuiltArea = request.MaxBuiltArea > 0 ? request.MaxBuiltArea : DefaultMaxArea;

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
            .WithFromRentalPrice(rentalPriceMin)
            .WithToRentalPrice(rentalPriceMax)
            .WithStatus(status)
            .WithRanking(ranking);

        return builder.Build();
    }
}