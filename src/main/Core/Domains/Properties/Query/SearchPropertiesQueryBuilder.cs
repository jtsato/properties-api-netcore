using System.Collections.Generic;
using System.Linq;
using Core.Commons.Models;
using Core.Domains.Properties.Models;

namespace Core.Domains.Properties.Query;

public class SearchPropertiesQueryBuilder
{
    private string _transaction;
    private byte _minBedrooms;
    private byte _maxBedrooms;
    private byte _minToilets;
    private byte _maxToilets;
    private byte _minGarages;
    private byte _maxGarages;
    private int _fromArea;
    private int _toArea;
    private int _minBuiltArea;
    private int _maxBuiltArea;
    private string _state;
    private string _city;
    private float _minSellingPrice;
    private float _toSellingPrice;
    private float _fromRentalTotalPrice;
    private float _toRentalTotalPrice;
    private string _status;
    private byte _ranking;

    private List<string> _types = new List<string>();
    private List<string> _districts = new List<string>();

    public SearchPropertiesQueryBuilder WithTypes(List<string> types)
    {
        _types = types;
        return this;
    }

    public SearchPropertiesQueryBuilder WithTransaction(string transaction)
    {
        _transaction = transaction;
        return this;
    }

    public SearchPropertiesQueryBuilder WithMinBedrooms(byte minBedrooms)
    {
        _minBedrooms = minBedrooms;
        return this;
    }

    public SearchPropertiesQueryBuilder WithMaxBedrooms(byte maxBedrooms)
    {
        _maxBedrooms = maxBedrooms;
        return this;
    }

    public SearchPropertiesQueryBuilder WithMinToilets(byte minToilets)
    {
        _minToilets = minToilets;
        return this;
    }

    public SearchPropertiesQueryBuilder WithMaxToilets(byte maxToilets)
    {
        _maxToilets = maxToilets;
        return this;
    }

    public SearchPropertiesQueryBuilder WithMinGarages(byte minGarages)
    {
        _minGarages = minGarages;
        return this;
    }

    public SearchPropertiesQueryBuilder WithMaxGarages(byte maxGarages)
    {
        _maxGarages = maxGarages;
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

    public SearchPropertiesQueryBuilder WithMinBuiltArea(int minBuiltArea)
    {
        _minBuiltArea = minBuiltArea;
        return this;
    }

    public SearchPropertiesQueryBuilder WithMaxBuiltArea(int maxBuiltArea)
    {
        _maxBuiltArea = maxBuiltArea;
        return this;
    }

    public SearchPropertiesQueryBuilder WithMinSellingPrice(float minSellingPrice)
    {
        _minSellingPrice = minSellingPrice;
        return this;
    }

    public SearchPropertiesQueryBuilder WithToSellingPrice(float toSellingPrice)
    {
        _toSellingPrice = toSellingPrice;
        return this;
    }

    public SearchPropertiesQueryBuilder WithFromRentalTotalPrice(float fromRentalTotalPrice)
    {
        _fromRentalTotalPrice = fromRentalTotalPrice;
        return this;
    }

    public SearchPropertiesQueryBuilder WithToRentalTotalPrice(float toRentalTotalPrice)
    {
        _toRentalTotalPrice = toRentalTotalPrice;
        return this;
    }

    public SearchPropertiesQueryBuilder WithStatus(string status)
    {
        _status = status;
        return this;
    }
    
    public SearchPropertiesQueryBuilder WithRanking(byte ranking)
    {
        _ranking = ranking;
        return this;
    }

    public SearchPropertiesQuery Build()
    {
        List<string> types = SanitizeTypes(_types);

        List<string> propertyTypes = types.Any() ? types : new List<string> {PropertyType.All.Name};

        string status = string.IsNullOrWhiteSpace(_status) ? "ALL" : _status.ToUpper();
        string transaction = string.IsNullOrWhiteSpace(_transaction) ? "ALL" : _transaction.ToUpper();

        SearchPropertiesQueryAdvertise queryAdvertise = new SearchPropertiesQueryAdvertise(transaction);
        SearchPropertiesQueryLocation queryLocation = new SearchPropertiesQueryLocation(_state, _city, _districts);

        SearchPropertiesQueryAttributes queryAttributes = new SearchPropertiesQueryAttributes
        (
            numberOfBedrooms: Range<byte>.Of(_minBedrooms, _maxBedrooms),
            numberOfToilets: Range<byte>.Of(_minToilets, _maxToilets),
            numberOfGarages: Range<byte>.Of(_minGarages, _maxGarages),
            area: Range<int>.Of(_fromArea, _toArea),
            builtArea: Range<int>.Of(_minBuiltArea, _maxBuiltArea)
        );

        SearchPropertiesQueryPrices queryPrices = new SearchPropertiesQueryPrices
        (
            sellingPrice: Range<float>.Of(_minSellingPrice, _toSellingPrice),
            rentalTotalPrice: Range<float>.Of(_fromRentalTotalPrice, _toRentalTotalPrice)
        );

        return new SearchPropertiesQuery(
            types: propertyTypes,
            advertise: queryAdvertise,
            attributes: queryAttributes,
            location: queryLocation,
            prices: queryPrices,
            status: status,
            ranking: _ranking
        );
    }

    private static List<string> SanitizeTypes(IEnumerable<string> types)
    {
        List<string> sanitizeTypes = types?
            .Select(PropertyType.GetByName)
            .Where(optional => optional.HasValue())
            .Select(optional => optional.GetValue().Name)
            .ToList();

        return sanitizeTypes?.Any() == true ? sanitizeTypes : new List<string> {PropertyType.All.Name};
    }
}