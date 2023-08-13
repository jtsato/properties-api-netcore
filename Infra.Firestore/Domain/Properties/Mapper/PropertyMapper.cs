using System.Linq;
using Core.Domains.Properties.Models;
using Infra.Firestore.Domain.Properties.Models;

namespace Infra.Firestore.Domain.Properties.Mapper;

public static class PropertyMapper
{
    public static Property Of(this PropertyEntity propertyEntity)
    {
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
            NumberOfBedrooms = propertyEntity.NumberOfBedrooms,
            NumberOfToilets = propertyEntity.NumberOfToilets,
            NumberOfGarages = propertyEntity.NumberOfGarages,
            Area = propertyEntity.Area,
            BuiltArea = propertyEntity.BuiltArea
        };

        PropertyLocation location = new PropertyLocation
        {
            City = propertyEntity.City,
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

        PropertyType type = PropertyType.GetByName(propertyEntity.Type).GetValue();

        return new Property
        {
            Id = propertyEntity.Id,
            Type = type,
            Advertise = advertise,
            Attributes = attributes,
            Location = location,
            Prices = prices,
            HashKey = propertyEntity.HashKey,
            CreatedAt = propertyEntity.CreatedAt,
            UpdatedAt = propertyEntity.UpdatedAt
        };
    }
}