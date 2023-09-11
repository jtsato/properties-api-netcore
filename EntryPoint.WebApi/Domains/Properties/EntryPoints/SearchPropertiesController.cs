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
        string type = PropertyType.GetByName(request.Type).OrElse(PropertyType.All).Name;
        string transaction = Transaction.GetByName(request.Transaction).OrElse(Transaction.All).Name;
        string status = PropertyStatus.GetByName(request.Status).OrElse(PropertyStatus.All).Name;

        SearchPropertiesQueryBuilder builder = new SearchPropertiesQueryBuilder();

        builder
            .WithType(type)
            .WithTransaction(transaction)
            .WithState(request.State)
            .WithCity(request.City)
            .WithDistricts(request.Districts)
            .WithFromNumberOfBedrooms(request.NumberOfBedroomsMin)
            .WithToNumberOfBedrooms(request.NumberOfBedroomsMax)
            .WithFromNumberOfToilets(request.NumberOfToiletsMin)
            .WithToNumberOfToilets(request.NumberOfToiletsMax)
            .WithFromNumberOfGarages(request.NumberOfGaragesMin)
            .WithToNumberOfGarages(request.NumberOfGaragesMax)
            .WithFromArea(request.AreaMin)
            .WithToArea(request.AreaMax)
            .WithFromBuiltArea(request.BuiltAreaMin)
            .WithToBuiltArea(request.BuiltAreaMax)
            .WithFromSellingPrice(request.SellingPriceMin)
            .WithToSellingPrice(request.SellingPriceMax)
            .WithFromRentalTotalPrice(request.RentalTotalPriceMin)
            .WithToRentalTotalPrice(request.RentalTotalPriceMax)
            .WithFromRentalPrice(request.RentalPriceMin)
            .WithToRentalPrice(request.RentalPriceMax)
            .WithFromPriceByM2(request.PriceByM2Min)
            .WithToPriceByM2(request.PriceByM2Max)
            .WithStatus(status);

        return builder.Build();
    }
}