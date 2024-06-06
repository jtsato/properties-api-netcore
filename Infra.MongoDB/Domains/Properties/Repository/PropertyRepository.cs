using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Infra.MongoDB.Commons.Connection;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.Properties.Model;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.Properties.Repository;

[ExcludeFromCodeCoverage]
public sealed class PropertyRepository : Repository<PropertyEntity>
{
    public PropertyRepository(IConnectionFactory connectionFactory, string databaseName, string collectionName)
        : base(connectionFactory, databaseName, collectionName)
    {
        IndexKeysDefinition<PropertyEntity> indexKeyId = Builders<PropertyEntity>
            .IndexKeys.Ascending(document => document.Id);
        
        IndexKeysDefinition<PropertyEntity> indexKeyUuid = Builders<PropertyEntity>
            .IndexKeys.Ascending(document => document.Uuid);

        IndexKeysDefinition<PropertyEntity> indexKeyTenantId = Builders<PropertyEntity>
            .IndexKeys.Ascending(document => document.TenantId);

        IndexKeysDefinition<PropertyEntity> indexKeyRefId = Builders<PropertyEntity>
            .IndexKeys.Ascending(document => document.RefId);

        IndexKeysDefinition<PropertyEntity> indexKeyTransaction = Builders<PropertyEntity>
            .IndexKeys.Ascending(document => document.Transaction);

        IndexKeysDefinition<PropertyEntity> indexKeyType = Builders<PropertyEntity>
            .IndexKeys.Ascending(document => document.Type);

        IndexKeysDefinition<PropertyEntity> indexKeyState = Builders<PropertyEntity>
            .IndexKeys.Ascending(document => document.State);

        IndexKeysDefinition<PropertyEntity> indexKeyCity = Builders<PropertyEntity>
            .IndexKeys.Ascending(document => document.City);

        IndexKeysDefinition<PropertyEntity> indexKeyDistrict = Builders<PropertyEntity>
            .IndexKeys.Ascending(document => document.District);

        IndexKeysDefinition<PropertyEntity> indexKeyRanking = Builders<PropertyEntity>
            .IndexKeys.Descending(document => document.Ranking);

        IndexKeysDefinition<PropertyEntity> indexKeyStatus = Builders<PropertyEntity>
            .IndexKeys.Ascending(document => document.Status);

        IndexKeysDefinition<PropertyEntity> indexKeyUpdateAt = Builders<PropertyEntity>
            .IndexKeys.Descending(document => document.UpdatedAt);

        IndexKeysDefinition<PropertyEntity> compositeDefaultIndex = Builders<PropertyEntity>
            .IndexKeys.Combine(indexKeyStatus, indexKeyRanking);

        IndexKeysDefinition<PropertyEntity> compositeTenantRefIndex = Builders<PropertyEntity>
            .IndexKeys.Combine(indexKeyTenantId, indexKeyRefId);

        GetCollection().Indexes
            .CreateManyAsync(
                new List<CreateIndexModel<PropertyEntity>>
                {
                    new CreateIndexModel<PropertyEntity>(indexKeyId, CreateUniqueIndexOptions("IDX_Property_Id")),
                    new CreateIndexModel<PropertyEntity>(indexKeyUuid, CreateUniqueIndexOptions("IDX_Property_Uuid")),
                    new CreateIndexModel<PropertyEntity>(indexKeyTransaction, CreateNonUniqueIndexOptions("IDX_Property_Transaction")),
                    new CreateIndexModel<PropertyEntity>(indexKeyType, CreateNonUniqueIndexOptions("IDX_Property_Type")),
                    new CreateIndexModel<PropertyEntity>(indexKeyState, CreateNonUniqueIndexOptions("IDX_Property_State")),
                    new CreateIndexModel<PropertyEntity>(indexKeyCity, CreateNonUniqueIndexOptions("IDX_Property_City")),
                    new CreateIndexModel<PropertyEntity>(indexKeyDistrict, CreateNonUniqueIndexOptions("IDX_Property_District")),
                    new CreateIndexModel<PropertyEntity>(indexKeyUpdateAt, CreateNonUniqueIndexOptions("IDX_Property_UpdatedAt")),
                    new CreateIndexModel<PropertyEntity>(compositeDefaultIndex, CreateNonUniqueIndexOptions("IDX_Property_Status_Ranking")),
                    new CreateIndexModel<PropertyEntity>(compositeTenantRefIndex, CreateUniqueIndexOptions("IDX_Property_TenantId_RefId"))
                });
    }
    
    private static CreateIndexOptions CreateUniqueIndexOptions(string name) =>
        new CreateIndexOptions
        {
            Name = name, Unique = true, Sparse = true, Background = false
        };
    
    private static CreateIndexOptions CreateNonUniqueIndexOptions(string name) =>
        new CreateIndexOptions
        {
            Name = name, Unique = false, Sparse = true, Background = false
        };
}