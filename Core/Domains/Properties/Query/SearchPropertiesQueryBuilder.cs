using System.Collections.Generic;
using Core.Commons.Models;

namespace Core.Domains.Properties.Query;

public class SearchPropertiesQueryBuilder
{
    private int _tenantId;
    private string _type;
    private string _transaction;
    private string _refId;
    private byte _fromNumberOfBedrooms;
    private byte _toNumberOfBedrooms;
    private byte _fromNumberOfToilets;
    private byte _toNumberOfToilets;
    private byte _fromNumberOfGarages;
    private byte _toNumberOfGarages;
    private int _fromArea;
    private int _toArea;
    private int _fromBuiltArea;
    private int _toBuiltArea;
    private string _state;
    private string _city;
    private decimal _fromSellingPrice;
    private decimal _toSellingPrice;
    private decimal _fromRentalTotalPrice;
    private decimal _toRentalTotalPrice;
    private decimal _fromRentalPrice;
    private decimal _toRentalPrice;
    private decimal _fromPriceByM2;
    private decimal _toPriceByM2;
    private int _fromRanking;
    private int _toRanking;
    private string _status;
    private string _fromCreatedAt;
    private string _toCreatedAt;
    private string _fromUpdatedAt;
    private string _toUpdatedAt;

    private List<string> _districts = new List<string>();
    
    public SearchPropertiesQueryBuilder WithTenantId(int tenantId)
    {
        _tenantId = tenantId;
        return this;
    }

    public SearchPropertiesQueryBuilder WithType(string type)
    {
        _type = type;
        return this;
    }

    public SearchPropertiesQueryBuilder WithTransaction(string transaction)
    {
        _transaction = transaction;
        return this;
    }

    public SearchPropertiesQueryBuilder WithRefId(string refId)
    {
        _refId = refId;
        return this;
    }

    public SearchPropertiesQueryBuilder WithFromNumberOfBedrooms(byte fromNumberOfBedrooms)
    {
        _fromNumberOfBedrooms = fromNumberOfBedrooms;
        return this;
    }

    public SearchPropertiesQueryBuilder WithToNumberOfBedrooms(byte toNumberOfBedrooms)
    {
        _toNumberOfBedrooms = toNumberOfBedrooms;
        return this;
    }

    public SearchPropertiesQueryBuilder WithFromNumberOfToilets(byte fromNumberOfToilets)
    {
        _fromNumberOfToilets = fromNumberOfToilets;
        return this;
    }

    public SearchPropertiesQueryBuilder WithToNumberOfToilets(byte toNumberOfToilets)
    {
        _toNumberOfToilets = toNumberOfToilets;
        return this;
    }

    public SearchPropertiesQueryBuilder WithFromNumberOfGarages(byte fromNumberOfGarages)
    {
        _fromNumberOfGarages = fromNumberOfGarages;
        return this;
    }

    public SearchPropertiesQueryBuilder WithToNumberOfGarages(byte toNumberOfGarages)
    {
        _toNumberOfGarages = toNumberOfGarages;
        return this;
    }

    public SearchPropertiesQueryBuilder WithCity(string city)
    {
        _city = city;
        return this;
    }
    
    public SearchPropertiesQueryBuilder WithState(string state)
    {
        _state = state;
        return this;
    }

    public SearchPropertiesQueryBuilder WithDistricts(List<string> districts)
    {
        _districts = districts;
        return this;
    }

    public SearchPropertiesQueryBuilder WithFromArea(int fromArea)
    {
        _fromArea = fromArea;
        return this;
    }

    public SearchPropertiesQueryBuilder WithToArea(int toArea)
    {
        _toArea = toArea;
        return this;
    }

    public SearchPropertiesQueryBuilder WithFromBuiltArea(int fromBuiltArea)
    {
        _fromBuiltArea = fromBuiltArea;
        return this;
    }

    public SearchPropertiesQueryBuilder WithToBuiltArea(int toBuiltArea)
    {
        _toBuiltArea = toBuiltArea;
        return this;
    }

    public SearchPropertiesQueryBuilder WithFromSellingPrice(decimal fromSellingPrice)
    {
        _fromSellingPrice = fromSellingPrice;
        return this;
    }

    public SearchPropertiesQueryBuilder WithToSellingPrice(decimal toSellingPrice)
    {
        _toSellingPrice = toSellingPrice;
        return this;
    }

    public SearchPropertiesQueryBuilder WithFromRentalTotalPrice(decimal fromRentalTotalPrice)
    {
        _fromRentalTotalPrice = fromRentalTotalPrice;
        return this;
    }

    public SearchPropertiesQueryBuilder WithToRentalTotalPrice(decimal toRentalTotalPrice)
    {
        _toRentalTotalPrice = toRentalTotalPrice;
        return this;
    }

    public SearchPropertiesQueryBuilder WithFromRentalPrice(decimal fromRentalPrice)
    {
        _fromRentalPrice = fromRentalPrice;
        return this;
    }

    public SearchPropertiesQueryBuilder WithToRentalPrice(decimal toRentalPrice)
    {
        _toRentalPrice = toRentalPrice;
        return this;
    }

    public SearchPropertiesQueryBuilder WithFromPriceByM2(decimal fromPriceByM2)
    {
        _fromPriceByM2 = fromPriceByM2;
        return this;
    }

    public SearchPropertiesQueryBuilder WithToPriceByM2(decimal toPriceByM2)
    {
        _toPriceByM2 = toPriceByM2;
        return this;
    }
    
    public SearchPropertiesQueryBuilder WithFromRanking(int fromRanking)
    {
        _fromRanking = fromRanking;
        return this;
    }
    
    public SearchPropertiesQueryBuilder WithToRanking(int toRanking)
    {
        _toRanking = toRanking;
        return this;
    }
    
    public SearchPropertiesQueryBuilder WithStatus(string status)
    {
        _status = status;
        return this;
    }

    public SearchPropertiesQueryBuilder WithFromCreatedAt(string fromCreatedAt)
    {
        _fromCreatedAt = fromCreatedAt;
        return this;
    }

    public SearchPropertiesQueryBuilder WithToCreatedAt(string toCreatedAt)
    {
        _toCreatedAt = toCreatedAt;
        return this;
    }

    public SearchPropertiesQueryBuilder WithFromUpdatedAt(string fromUpdatedAt)
    {
        _fromUpdatedAt = fromUpdatedAt;
        return this;
    }

    public SearchPropertiesQueryBuilder WithToUpdatedAt(string toUpdatedAt)
    {
        _toUpdatedAt = toUpdatedAt;
        return this;
    }

    public SearchPropertiesQuery Build()
    {
        int tenantId = _tenantId;
        string propertyType = _type ?? "All";
        
        SearchPropertiesQueryAdvertise queryAdvertise = new SearchPropertiesQueryAdvertise(_transaction, _refId);
        SearchPropertiesQueryLocation queryLocation = new SearchPropertiesQueryLocation(_state, _city, _districts);

        SearchPropertiesQueryAttributes queryAttributes = new SearchPropertiesQueryAttributes
        {
            NumberOfBedrooms = Range<byte>.Of(_fromNumberOfBedrooms, _toNumberOfBedrooms),
            NumberOfToilets = Range<byte>.Of(_fromNumberOfToilets, _toNumberOfToilets),
            NumberOfGarages = Range<byte>.Of(_fromNumberOfGarages, _toNumberOfGarages),
            Area = Range<int>.Of(_fromArea, _toArea),
            BuiltArea = Range<int>.Of(_fromBuiltArea, _toBuiltArea)
        };

        SearchPropertiesQueryPrices queryPrices = new SearchPropertiesQueryPrices
        {
            SellingPrice = Range<decimal>.Of(_fromSellingPrice, _toSellingPrice),
            RentalTotalPrice = Range<decimal>.Of(_fromRentalTotalPrice, _toRentalTotalPrice),
            RentalPrice = Range<decimal>.Of(_fromRentalPrice, _toRentalPrice),
            PriceByM2 = Range<decimal>.Of(_fromPriceByM2, _toPriceByM2)
        };
        
        SearchPropertiesQueryRanking queryRanking = new SearchPropertiesQueryRanking
        {
            Ranking = Range<int>.Of(_fromRanking, _toRanking)
        };
        
        string status = _status;
        
        return new SearchPropertiesQuery(
            tenantId,
            propertyType,
            queryAdvertise,
            queryAttributes,
            queryLocation,
            queryPrices,
            queryRanking,
            status,
            Range<string>.Of(_fromCreatedAt, _toCreatedAt),
            Range<string>.Of(_fromUpdatedAt, _toUpdatedAt)
        );
    }
}
