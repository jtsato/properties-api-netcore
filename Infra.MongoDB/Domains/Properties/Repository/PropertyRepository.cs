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
            .IndexKeys.Combine(indexKeyTransaction, indexKeyRanking, indexKeyStatus);

        IndexKeysDefinition<PropertyEntity> compositeTenantRefIndex = Builders<PropertyEntity>
            .IndexKeys.Combine(indexKeyTenantId, indexKeyRefId);

        CreateIndexOptions uniqueIndexOptions = new CreateIndexOptions
            {Unique = true, Sparse = true, Background = false};

        CreateIndexOptions nonUniqueIndexOptions = new CreateIndexOptions
            {Unique = false, Sparse = true, Background = false};

        GetCollection().Indexes
            .CreateMany(
                new List<CreateIndexModel<PropertyEntity>>
                {
                    new CreateIndexModel<PropertyEntity>(indexKeyId, uniqueIndexOptions),
                    new CreateIndexModel<PropertyEntity>(indexKeyTransaction, nonUniqueIndexOptions),
                    new CreateIndexModel<PropertyEntity>(indexKeyType, nonUniqueIndexOptions),
                    new CreateIndexModel<PropertyEntity>(indexKeyState, nonUniqueIndexOptions),
                    new CreateIndexModel<PropertyEntity>(indexKeyCity, nonUniqueIndexOptions),
                    new CreateIndexModel<PropertyEntity>(indexKeyDistrict, nonUniqueIndexOptions),
                    new CreateIndexModel<PropertyEntity>(indexKeyRanking, nonUniqueIndexOptions),
                    new CreateIndexModel<PropertyEntity>(indexKeyStatus, nonUniqueIndexOptions),
                    new CreateIndexModel<PropertyEntity>(indexKeyUpdateAt, nonUniqueIndexOptions),
                    new CreateIndexModel<PropertyEntity>(compositeDefaultIndex, nonUniqueIndexOptions),
                    new CreateIndexModel<PropertyEntity>(compositeTenantRefIndex, uniqueIndexOptions)
                });
    }
}