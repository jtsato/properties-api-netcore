using System;
using Google.Cloud.Firestore;
using Infra.Firestore.Commons.Repository;

namespace Infra.Firestore.Domain.Properties.Models;

[FirestoreData]
public class PropertyEntity : Entity
{
    [FirestoreProperty(name: "id")] 
    public string Id { get; set; }

    [FirestoreProperty(name: "type")] 
    public string Type { get; set; } // Pending

    [FirestoreProperty(name: "tenantId")] 
    public int TenantId { get; set; }

    [FirestoreProperty(name: "transaction")]
    public string Transaction { get; set; }

    [FirestoreProperty(name: "title")] 
    public string Title { get; set; }

    [FirestoreProperty(name: "description")]
    public string Description { get; set; }

    [FirestoreProperty(name: "url")] 
    public string Url { get; set; }

    [FirestoreProperty(name: "refId")] 
    public string RefId { get; set; }

    [FirestoreProperty(name: "images")] 
    public string[] Images { get; set; }

    [FirestoreProperty(name: "numberOfBedrooms")]
    public byte NumberOfBedrooms { get; set; }

    [FirestoreProperty(name: "numberOfToilets")]
    public byte NumberOfToilets { get; set; }

    [FirestoreProperty(name: "numberOfGarages")]
    public byte NumberOfGarages { get; set; }

    [FirestoreProperty(name: "area")] 
    public int Area { get; set; }

    [FirestoreProperty(name: "builtArea")] 
    public int BuiltArea { get; set; }

    [FirestoreProperty(name: "state")] 
    public string City { get; set; } // Pending

    [FirestoreProperty(name: "district")] 
    public string District { get; set; }

    [FirestoreProperty(name: "address")] 
    public string Address { get; set; }

    [FirestoreProperty(name: "sellingPrice")]
    public decimal SellingPrice { get; set; }

    [FirestoreProperty(name: "rentalTotalPrice")]
    public decimal RentalTotalPrice { get; set; }

    [FirestoreProperty(name: "rentalPrice")]
    public decimal RentalPrice { get; set; }

    [FirestoreProperty(name: "discount")] 
    public decimal Discount { get; set; }

    [FirestoreProperty(name: "condominiumFee")]
    public decimal CondominiumFee { get; set; }

    [FirestoreProperty(name: "priceByM2")] 
    public decimal PriceByM2 { get; set; }

    [FirestoreProperty(name: "hashKey")]
    public string HashKey { get; set; }

    [FirestoreProperty(name: "createdAt")] 
    public DateTime CreatedAt { get; set; }

    [FirestoreProperty(name: "updatedAt")] 
    public DateTime UpdatedAt { get; set; }
}