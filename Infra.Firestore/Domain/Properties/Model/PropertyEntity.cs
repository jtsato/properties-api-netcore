using Google.Cloud.Firestore;
using Infra.Firestore.Commons.Repository;

namespace Infra.Firestore.Domain.Properties.Model;

// ReSharper disable once ClassNeverInstantiated.Global
[FirestoreData]
public class PropertyEntity : Entity
{
    public new string Id => Reference.Id;
    
    [FirestoreDocumentId]
    public DocumentReference Reference { get; set; }    

    [FirestoreProperty(name: "type")] 
    public string Type { get; set; }

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
    // TODO: Fix the images attribute NOT mapping from Firestore to C#.

    [FirestoreProperty(name: "numberOfBedrooms")]
    public int NumberOfBedrooms { get; set; }

    [FirestoreProperty(name: "numberOfToilets")]
    public int NumberOfToilets { get; set; }

    [FirestoreProperty(name: "numberOfGarages")]
    public int NumberOfGarages { get; set; }

    [FirestoreProperty(name: "area")] 
    public int Area { get; set; }

    [FirestoreProperty(name: "builtArea")] 
    public int BuiltArea { get; set; }

    [FirestoreProperty(name: "city")] 
    public string City { get; set; }
    
    [FirestoreProperty(name: "state")]
    public string State { get; set; }

    [FirestoreProperty(name: "district")] 
    public string District { get; set; }

    [FirestoreProperty(name: "address")] 
    public string Address { get; set; }

    [FirestoreProperty(name: "sellingPrice", ConverterType = typeof(System.Type))]
    public double SellingPrice { get; set; }

    [FirestoreProperty(name: "rentalTotalPrice")]
    public double RentalTotalPrice { get; set; }

    [FirestoreProperty(name: "rentalPrice")]
    public double RentalPrice { get; set; }

    [FirestoreProperty(name: "discount")] 
    public double Discount { get; set; }

    [FirestoreProperty(name: "condominiumFee")]
    public double CondominiumFee { get; set; }

    [FirestoreProperty(name: "priceByM2")] 
    public double PriceByM2 { get; set; }

    [FirestoreProperty(name: "hashKey")]
    public string HashKey { get; set; }
    
    [FirestoreProperty(name: "ranking")]
    public int Ranking { get; set; }
    
    [FirestoreProperty(name: "status")]
    public string Status { get; set; }

    [FirestoreProperty(name: "createdAt")] 
    public Timestamp  CreatedAt { get; set; }

    [FirestoreProperty(name: "updatedAt")] 
    public Timestamp  UpdatedAt { get; set; }
}