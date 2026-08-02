using System;
using System.Diagnostics.CodeAnalysis;
using Infra.MongoDB.Commons.Repository;
using MongoDB.Bson.Serialization.Attributes;

namespace Infra.MongoDB.Domains.Properties.Model;

[ExcludeFromCodeCoverage]
[BsonIgnoreExtraElements]
[Serializable]
public class PropertyEntity : Entity
{
    [BsonElement("uuid")]
    public string Uuid { get; set; }
    
    [BsonElement("type")]
    public string Type { get; set; }

    [BsonElement("tenantId")]
    public int TenantId { get; set; }

    [BsonElement("transaction")]
    public string Transaction { get; set; }

    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("url")]
    public string Url { get; set; }

    [BsonElement("refId")]
    public string RefId { get; set; }

    [BsonElement("images")]
    public string[] Images { get; set; }
    
    [BsonElement("hdImages")]
    public string[] HdImages { get; set; }

    [BsonElement("numberOfBedrooms")]
    public int NumberOfBedrooms { get; set; }

    [BsonElement("numberOfToilets")]
    public int NumberOfToilets { get; set; }

    [BsonElement("numberOfGarages")]
    public int NumberOfGarages { get; set; }

    [BsonElement("area")]
    public int Area { get; set; }

    [BsonElement("builtArea")]
    public int BuiltArea { get; set; }

    [BsonElement("city")]
    public string City { get; set; }

    [BsonElement("state")]
    public string State { get; set; }

    [BsonElement("district")]
    public string District { get; set; }

    [BsonElement("address")]
    public string Address { get; set; }

    [BsonElement("sellingPrice")]
    public double SellingPrice { get; set; }

    [BsonElement("rentalTotalPrice")]
    public double RentalTotalPrice { get; set; }

    [BsonElement("rentalPrice")]
    public double RentalPrice { get; set; }

    [BsonElement("discount")]
    public double Discount { get; set; }

    [BsonElement("condominiumFee")]
    public double CondominiumFee { get; set; }

    [BsonElement("priceByM2")]
    public double PriceByM2 { get; set; }

    [BsonElement("hashKey")]
    public string HashKey { get; set; }

    [BsonElement("ranking")]
    public int Ranking { get; set; }

    [BsonElement("status")]
    public string Status { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}