using System.Collections.Generic;
using Core.Commons.Models;

namespace Core.Domains.Properties.Query;

public class SearchPropertiesQueryBuilder
{
    private string _type;
    private string _transaction;
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
    private double _fromSellingPrice;
    private double _toSellingPrice;
    private double _fromRentalTotalPrice;
    private double _toRentalTotalPrice;
    private double _fromRentalPrice;
    private double _toRentalPrice;
    private double _fromPriceByM2;
    private double _toPriceByM2;
    private string _status;

    private List<string> _districts = new List<string>();

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

    public SearchPropertiesQueryBuilder WithFromSellingPrice(double fromSellingPrice)
    {
        _fromSellingPrice = fromSellingPrice;
        return this;
    }

    public SearchPropertiesQueryBuilder WithToSellingPrice(double toSellingPrice)
    {
        _toSellingPrice = toSellingPrice;
        return this;
    }

    public SearchPropertiesQueryBuilder WithFromRentalTotalPrice(double fromRentalTotalPrice)
    {
        _fromRentalTotalPrice = fromRentalTotalPrice;
        return this;
    }

    public SearchPropertiesQueryBuilder WithToRentalTotalPrice(double toRentalTotalPrice)
    {
        _toRentalTotalPrice = toRentalTotalPrice;
        return this;
    }

    public SearchPropertiesQueryBuilder WithFromRentalPrice(double fromRentalPrice)
    {
        _fromRentalPrice = fromRentalPrice;
        return this;
    }

    public SearchPropertiesQueryBuilder WithToRentalPrice(double toRentalPrice)
    {
        _toRentalPrice = toRentalPrice;
        return this;
    }

    public SearchPropertiesQueryBuilder WithFromPriceByM2(double fromPriceByM2)
    {
        _fromPriceByM2 = fromPriceByM2;
        return this;
    }

    public SearchPropertiesQueryBuilder WithToPriceByM2(double toPriceByM2)
    {
        _toPriceByM2 = toPriceByM2;
        return this;
    }

    public SearchPropertiesQueryBuilder WithStatus(string status)
    {
        _status = status;
        return this;
    }

    public SearchPropertiesQuery Build()
    {
        string propertyType = _type ?? "All";

        SearchPropertiesQueryAdvertise queryAdvertise = new SearchPropertiesQueryAdvertise(_transaction);
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
            SellingPrice = Range<double>.Of(_fromSellingPrice, _toSellingPrice),
            RentalTotalPrice = Range<double>.Of(_fromRentalTotalPrice, _toRentalTotalPrice),
            RentalPrice = Range<double>.Of(_fromRentalPrice, _toRentalPrice),
            PriceByM2 = Range<double>.Of(_fromPriceByM2, _toPriceByM2)
        };

        string status = _status ?? "NONE";

        return new SearchPropertiesQuery(
            type: propertyType,
            advertise: queryAdvertise,
            attributes: queryAttributes,
            location: queryLocation,
            prices: queryPrices,
            status: status
        );
    }
}