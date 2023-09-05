using System.Linq;
using Core.Domains.Properties.Models;
using Infra.MongoDB.Domains.Properties.Model;

namespace Infra.MongoDB.Domains.Properties.Mapper;

public static class PropertyMapper
{
    public static Property Map(this PropertyEntity propertyEntity)
    {
        PropertyType type = PropertyType.GetByName(propertyEntity.Type).GetValue();

        Transaction transaction = Transaction.GetByName(propertyEntity.Transaction).GetValue();

        PropertyAdvertise advertise = new PropertyAdvertise
        {
            TenantId = propertyEntity.TenantId,
            Transaction = transaction,
            Title = propertyEntity.Title,
            Description = propertyEntity.Description,
            Url = propertyEntity.Url,
            RefId = propertyEntity.RefId,
            Images = propertyEntity.Images.ToList()
        };

        PropertyAttributes attributes = new PropertyAttributes
        {
            NumberOfBedrooms = (byte) propertyEntity.NumberOfBedrooms,
            NumberOfToilets = (byte) propertyEntity.NumberOfToilets,
            NumberOfGarages = (byte) propertyEntity.NumberOfGarages,
            Area = propertyEntity.Area,
            BuiltArea = propertyEntity.BuiltArea
        };

        PropertyLocation location = new PropertyLocation
        {
            City = propertyEntity.City,
            State = propertyEntity.State,
            District = propertyEntity.District,
            Address = propertyEntity.Address
        };

        PropertyPrices prices = new PropertyPrices
        {
            SellingPrice = propertyEntity.SellingPrice,
            RentalTotalPrice = propertyEntity.RentalTotalPrice,
            RentalPrice = propertyEntity.RentalPrice,
            Discount = propertyEntity.Discount,
            CondominiumFee = propertyEntity.CondominiumFee,
            PriceByM2 = propertyEntity.PriceByM2
        };

        PropertyStatus status = PropertyStatus.GetByName(propertyEntity.Status).GetValue();

        return new Property
        {
            Id = propertyEntity.Id,
            Type = type,
            Advertise = advertise,
            Attributes = attributes,
            Location = location,
            Prices = prices,
            HashKey = propertyEntity.HashKey,
            Ranking = (byte) propertyEntity.Ranking,
            Status = status,
            CreatedAt = propertyEntity.CreatedAt.ToLocalTime(),
            UpdatedAt = propertyEntity.UpdatedAt.ToLocalTime()
        };
    }

    public static PropertyEntity Map(this Property property)
    {
        return new PropertyEntity
        {
            Type = property.Type.Name,
            TenantId = property.Advertise.TenantId,
            Transaction = property.Advertise.Transaction.Name,
            Title = property.Advertise.Title,
            Description = property.Advertise.Description,
            Url = property.Advertise.Url,
            RefId = property.Advertise.RefId,
            Images = property.Advertise.Images.ToArray(),
            NumberOfBedrooms = property.Attributes.NumberOfBedrooms,
            NumberOfToilets = property.Attributes.NumberOfToilets,
            NumberOfGarages = property.Attributes.NumberOfGarages,
            Area = property.Attributes.Area,
            BuiltArea = property.Attributes.BuiltArea,
            City = property.Location.City,
            State = property.Location.State,
            District = property.Location.District,
            Address = property.Location.Address,
            SellingPrice = property.Prices.SellingPrice,
            RentalTotalPrice = property.Prices.RentalTotalPrice,
            RentalPrice = property.Prices.RentalPrice,
            Discount = property.Prices.Discount,
            CondominiumFee = property.Prices.CondominiumFee,
            PriceByM2 = property.Prices.PriceByM2,
            HashKey = property.HashKey,
            Ranking = property.Ranking,
            Status = property.Status.Name,
            CreatedAt = property.CreatedAt.ToUniversalTime(),
            UpdatedAt = property.UpdatedAt.ToUniversalTime()
        };
    }
}
