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
    private static readonly string[] SortableFields = {"sellingPrice", "rentalTotalPrice", "rentalPrice", "priceByM2", "createdAt", "updatedAt"};

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISearchPropertiesUseCase _useCase;
    private const int DefaultMaxPrice = 999999999;

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

        SearchPropertiesQueryBuilder builder = new SearchPropertiesQueryBuilder();

        bool isSale = transaction.Is(Transaction.All) || transaction.Is(Transaction.Sale);
        bool isRent = transaction.Is(Transaction.All) || transaction.Is(Transaction.Rent);

        double sellingPriceMin = isSale ? request.MinPrice : 0;
        double sellingPriceMax = isSale ? request.MaxPrice : DefaultMaxPrice;
        double rentalPriceMin = isRent ? request.MinPrice : 0;
        double rentalPriceMax = isRent ? request.MaxPrice : DefaultMaxPrice;

        List<string> districts = request.Districts?
            .Where(element => !string.IsNullOrEmpty(element))
            .SelectMany(element => element.Split(','))
            .ToList();

        builder
            .WithTypes(types)
            .WithTransaction(transaction.Name)
            .WithState(request.Uf)
            .WithCity(request.City)
            .WithDistricts(districts)
            .WithMinBedrooms(request.MinBedrooms)
            .WithMaxBedrooms(request.MaxBedrooms)
            .WithMinToilets(request.MinToilets)
            .WithMaxToilets(request.MaxToilets)
            .WithMinGarages(request.MinGarages)
            .WithMaxGarages(request.MaxGarages)
            .WithFromArea(request.MinArea)
            .WithToArea(request.MaxArea)
            .WithMinBuiltArea(request.MinBuiltArea)
            .WithMaxBuiltArea(request.MaxBuiltArea)
            .WithMinSellingPrice(sellingPriceMin)
            .WithToSellingPrice(sellingPriceMax)
            .WithFromRentalPrice(rentalPriceMin)
            .WithToRentalPrice(rentalPriceMax)
            .WithStatus(status);

        return builder.Build();
    }
}