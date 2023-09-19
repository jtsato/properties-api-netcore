using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;
using Core.Domains.Properties.UseCases;
using EntryPoint.WebApi.Domains.Commons;
using EntryPoint.WebApi.Domains.Properties.EntryPoints;
using IntegrationTest.EntryPoint.WebApi.Commons;
using IntegrationTest.EntryPoint.WebApi.Commons.Assertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.EntryPoint.WebApi.Domains.Properties.EntryPoints;

[Collection("WebApi Collection Context")]
public class GetPropertyByIdApiMethodTest
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly ApiMethodInvoker _invoker;
    private readonly GetPropertyByIdApiMethod _apiMethod;
    private readonly Mock<IGetPropertyByIdUseCase> _useCaseMock;

    public GetPropertyByIdApiMethodTest(ITestOutputHelper outputHelper, ApiMethodInvokerHolder apiMethodInvokerHolder)
    {
        _outputHelper = outputHelper;
        _invoker = apiMethodInvokerHolder.GetApiMethodInvoker();
        _useCaseMock = new Mock<IGetPropertyByIdUseCase>(MockBehavior.Strict);
        IGetPropertyByIdController controller = new GetPropertyByIdController(_useCaseMock.Object);
        _apiMethod = new GetPropertyByIdApiMethod(controller);
    }

    [UseCulture("en-US")]
    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "GET /api/properties/{id} should return 500 Internal Server Error when use case throws an exception")]
    public async Task FailedToGetByIdReturningInternalServerError()
    {
        // Arrange
        _useCaseMock.Setup(useCase => useCase.ExecuteAsync(new GetPropertyByIdQuery("1001")))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        ObjectResult objectResult = await _invoker.InvokeAsync(() => _apiMethod.GetPropertyById("1001"));

        // Assert
        Assert.NotNull(objectResult);
        Assert.Equal((int) HttpStatusCode.InternalServerError, objectResult.StatusCode);

        JsonElement jsonElement = ApiMethodTestHelper.TryGetJsonElement(objectResult);

        const string errorMessage = "An unexpected error has occurred, please try again later!";

        JsonAssertHelper.AssertThat(jsonElement)
            .AndExpectThat(JsonFrom.Path("$.code"), Is<int>.EqualTo(500))
            .AndExpectThat(JsonFrom.Path("$.message"), Is<string>.EqualTo(errorMessage))
            .AndExpectThat(JsonFrom.Path("$.fields"), Is<object>.Empty());
    }
    
    [UseCulture("pt-BR")]
    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "GET /api/properties/{id} should return 500 Internal Server Error when use case throws an exception")]
    public async Task FailedToGetByIdReturningInternalServerErrorInPtBr()
    {
        // Arrange
        _useCaseMock.Setup(useCase => useCase.ExecuteAsync(new GetPropertyByIdQuery("1001")))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        ObjectResult objectResult = await _invoker.InvokeAsync(() => _apiMethod.GetPropertyById("1001"), "pt-BR");

        // Assert
        Assert.NotNull(objectResult);
        Assert.Equal((int) HttpStatusCode.InternalServerError, objectResult.StatusCode);

        JsonElement jsonElement = ApiMethodTestHelper.TryGetJsonElement(objectResult);

        const string errorMessage = "Ocorreu um erro inesperado, por favor tente novamente mais tarde!";

        JsonAssertHelper.AssertThat(jsonElement)
            .AndExpectThat(JsonFrom.Path("$.code"), Is<int>.EqualTo(500))
            .AndExpectThat(JsonFrom.Path("$.message"), Is<string>.EqualTo(errorMessage))
            .AndExpectThat(JsonFrom.Path("$.fields"), Is<object>.Empty());
    }

    [UseCulture("en-US")]
    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "GET /api/properties/{id} should return 200 OK")]
    public async Task SuccessfulToGetByIdReturningProperty()
    {
        // Arrange
        _useCaseMock.Setup(useCase => useCase.ExecuteAsync(new GetPropertyByIdQuery("1001")))
            .ReturnsAsync(
                new Property
                {
                    Id = 1001,
                    Type = PropertyType.Apartment,
                    Advertise = new PropertyAdvertise
                    {
                        TenantId = 1,
                        Transaction = Transaction.Rent,
                        Title = "Apartment for rent",
                        Description = "Apartment for rent",
                        Url = "https://www.apartment-for-rent.com",
                        RefId = "REF 101",
                        Images = new List<string>
                        {
                            "https://www.apartment-for-rent.com/image1.jpg",
                            "https://www.apartment-for-rent.com/image2.jpg"
                        }
                    },
                    Attributes = new PropertyAttributes
                    {
                        NumberOfBedrooms = 2,
                        NumberOfToilets = 1,
                        NumberOfGarages = 1,
                        Area = 100,
                        BuiltArea = 200,
                    },
                    Location = new PropertyLocation
                    {
                        State = "Duckland",
                        City = "White Duck",
                        District = "Downtown",
                        Address = "Good Life Street, 101",
                    },
                    Prices = new PropertyPrices
                    {
                        SellingPrice = 100000,
                        RentalTotalPrice = 1000,
                        RentalPrice = 700,
                        PriceByM2 = 100,
                        Discount = 100,
                        CondominiumFee = 90,
                    },
                    HashKey = "hash-key-1",
                    Ranking = 1,
                    Status = PropertyStatus.Active,
                    CreatedAt = DateTime.Parse("2023-01-01 23:59:59.999", CultureInfo.DefaultThreadCurrentCulture),
                    UpdatedAt = DateTime.Parse("2023-02-01 23:59:59.999", CultureInfo.DefaultThreadCurrentCulture),
                }
            );


        // Act
        ObjectResult objectResult = await _invoker.InvokeAsync(() => _apiMethod.GetPropertyById("1001"));

        // Assert
        Assert.NotNull(objectResult);
        Assert.Equal((int) HttpStatusCode.OK, objectResult.StatusCode);

        JsonElement jsonElement = ApiMethodTestHelper.TryGetJsonElement(objectResult);

        _outputHelper.WriteLine(jsonElement.ToString());

        JsonAssertHelper.AssertThat(jsonElement)
            .AndExpectThat(JsonFrom.Path("$.id"), Is<long>.EqualTo(1001))
            .AndExpectThat(JsonFrom.Path("$.tenantId"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.transaction"), Is<string>.EqualTo("RENT"))
            .AndExpectThat(JsonFrom.Path("$.title"), Is<string>.EqualTo("Apartment for rent"))
            .AndExpectThat(JsonFrom.Path("$.description"), Is<string>.EqualTo("Apartment for rent"))
            .AndExpectThat(JsonFrom.Path("$.url"), Is<string>.EqualTo("https://www.apartment-for-rent.com"))
            .AndExpectThat(JsonFrom.Path("$.refId"), Is<string>.EqualTo("REF 101"))
            .AndExpectThat(JsonFrom.Path("$.images"), Is<List<string>>.EqualTo(new List<string>
            {
                "https://www.apartment-for-rent.com/image1.jpg",
                "https://www.apartment-for-rent.com/image2.jpg"
            }))
            .AndExpectThat(JsonFrom.Path("$.numberOfBedrooms"), Is<int>.EqualTo(2))
            .AndExpectThat(JsonFrom.Path("$.numberOfToilets"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.numberOfGarages"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.area"), Is<byte>.EqualTo(100))
            .AndExpectThat(JsonFrom.Path("$.builtArea"), Is<int>.EqualTo(200))
            .AndExpectThat(JsonFrom.Path("$.state"), Is<string>.EqualTo("Duckland"))
            .AndExpectThat(JsonFrom.Path("$.city"), Is<string>.EqualTo("White Duck"))
            .AndExpectThat(JsonFrom.Path("$.district"), Is<string>.EqualTo("Downtown"))
            .AndExpectThat(JsonFrom.Path("$.address"), Is<string>.EqualTo("Good Life Street, 101"))
            .AndExpectThat(JsonFrom.Path("$.sellingPrice"), Is<int>.EqualTo(100000))
            .AndExpectThat(JsonFrom.Path("$.rentalTotalPrice"), Is<int>.EqualTo(1000))
            .AndExpectThat(JsonFrom.Path("$.rentalPrice"), Is<int>.EqualTo(700))
            .AndExpectThat(JsonFrom.Path("$.priceByM2"), Is<int>.EqualTo(100))
            .AndExpectThat(JsonFrom.Path("$.discount"), Is<int>.EqualTo(100))
            .AndExpectThat(JsonFrom.Path("$.condominiumFee"), Is<int>.EqualTo(90))
            .AndExpectThat(JsonFrom.Path("$.ranking"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.status"), Is<string>.EqualTo("ACTIVE"))
            .AndExpectThat(JsonFrom.Path("$.createdAt"), Is<string>.EqualTo("2023-01-01T23:59:59.999"))
            .AndExpectThat(JsonFrom.Path("$.updatedAt"), Is<string>.EqualTo("2023-02-01T23:59:59.999"));
    }
}