using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Commons.Paging;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;
using Core.Domains.Properties.UseCases;
using EntryPoint.WebApi.Commons;
using EntryPoint.WebApi.Commons.Models;
using EntryPoint.WebApi.Domains.Properties.EntryPoints;
using EntryPoint.WebApi.Domains.Properties.Models;
using IntegrationTest.EntryPoint.WebApi.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Domains.Properties.EntryPoints;

[Collection("WebApi Collection")]
public class SearchPropertiesApiMethodTest
{
    private readonly ApiMethodInvoker _invoker;
    private readonly SearchPropertiesApiMethod _apiMethod;
    private readonly Mock<ISearchPropertiesUseCase> _useCaseMock;

    public SearchPropertiesApiMethodTest(ApiMethodInvokerHolder apiMethodInvokerHolder)
    {
        _invoker = apiMethodInvokerHolder.GetApiMethodInvoker();
        _useCaseMock = new Mock<ISearchPropertiesUseCase>(MockBehavior.Strict);
        SearchPropertiesController controller = new SearchPropertiesController(
            httpContextAccessor: BuildHttpContextAccessorMock().Object,
            useCase: _useCaseMock.Object
        );
        _apiMethod = new SearchPropertiesApiMethod(controller);
    }

    private static Mock<IHttpContextAccessor> BuildHttpContextAccessorMock()
    {
        Mock<IHttpContextAccessor> httpContextAccessorMock = new Mock<IHttpContextAccessor>(MockBehavior.Strict);

        httpContextAccessorMock
            .Setup(contextAccessor => contextAccessor.HttpContext.Request.Scheme)
            .Returns("http");

        httpContextAccessorMock
            .Setup(contextAccessor => contextAccessor.HttpContext.Request.Host)
            .Returns(new HostString("localhost", 7029));

        httpContextAccessorMock
            .Setup(contextAccessor => contextAccessor.HttpContext.Request.PathBase)
            .Returns("/api/properties-search");

        httpContextAccessorMock
            .Setup(contextAccessor => contextAccessor.HttpContext.Request.Path)
            .Returns("/v1/properties");

        return httpContextAccessorMock;
    }

    [Trait("Category", "Entrypoint (WebApi) Integration tests")]
    [Fact(DisplayName = "GET /api/properties/search should return 200")]
    public async Task SuccessfulToSearchProperties()
    {
        // Arrange
        SearchPropertiesQueryBuilder queryBuilder = new SearchPropertiesQueryBuilder();

        queryBuilder
            .WithType("Apartment")
            .WithTransaction("Rent")
            .WithRefId("REF 101")
            .WithCity("White Duck")
            .WithDistricts(new List<string> {"Downtown", "Alta Vista"})
            .WithFromNumberOfBedrooms(0)
            .WithToNumberOfBedrooms(3)
            .WithFromNumberOfToilets(1)
            .WithToNumberOfToilets(2)
            .WithFromNumberOfGarages(1)
            .WithToNumberOfGarages(1)
            .WithFromSellingPrice(100000)
            .WithToSellingPrice(200000)
            .WithFromRentalTotalPrice(1000)
            .WithToRentalTotalPrice(3000)
            .WithFromRentalPrice(1000)
            .WithToRentalPrice(2000)
            .WithFromPriceByM2(100)
            .WithToPriceByM2(200)
            .WithFromCreatedAt("2023-01-01")
            .WithToCreatedAt("2023-02-01")
            .WithFromUpdatedAt("2023-02-01")
            .WithToUpdatedAt("2023-03-01");

        SearchPropertiesQuery query = queryBuilder.Build();

        _useCaseMock
            .Setup(useCase => useCase.ExecuteAsync(query, PageRequestHelper.Of("0", "3", "UpdatedDate:Desc")))
            .ReturnsAsync(new Page<Property>(new List<Property>(), new Pageable(0, 3, 3, 5, 2)));

        SearchPropertiesRequest request = new SearchPropertiesRequest
        {
            Type = "Apartment",
            Transaction = "Rent",
            RefId = "REF 101",
            City = "White Duck",
            Districts = new List<string> {"Downtown", "Alta Vista"},
            NumberOfBedroomsMin = 0,
            NumberOfBedroomsMax = 3,
            NumberOfToiletsMin = 1,
            NumberOfToiletsMax = 2,
            NumberOfGaragesMin = 1,
            NumberOfGaragesMax = 1,
            SellingPriceMin = 100000,
            SellingPriceMax = 200000,
            RentalTotalPriceMin = 1000,
            RentalTotalPriceMax = 3000,
            RentalPriceMin = 1000,
            RentalPriceMax = 2000,
            PriceByM2Min = 100,
            PriceByM2Max = 200,
            FromCreatedAt = "2023-01-01",
            ToCreatedAt = "2023-02-01",
            FromUpdatedAt = "2023-02-01",
            ToUpdatedAt = "2023-03-01"
        };

        QPageRequest qPageRequest = new QPageRequest
        {
            PageNumber = "0",
            PageSize = "3",
            OrderBy = new List<string> {"UpdatedDate,Desc"}
        };
        // Act
        ObjectResult result = await _invoker.InvokeAsync(() => _apiMethod.SearchProperties(request, qPageRequest));

        // Assert
        Assert.Equal(206, result.StatusCode);
    }
}