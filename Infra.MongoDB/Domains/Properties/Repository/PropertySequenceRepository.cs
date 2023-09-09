using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Infra.MongoDB.Commons.Connection;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.Properties.Model;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.Properties.Repository;

[ExcludeFromCodeCoverage]
public sealed class PropertySequenceRepository : ISequenceRepository<PropertySequence>
{
    private readonly IMongoCollection<PropertySequence> _collection;

    public PropertySequenceRepository(IConnectionFactory connectionFactory, string databaseName, string collectionName)
    {
        IMongoDatabase database = connectionFactory.GetDatabase(databaseName);
        _collection = database.GetCollection<PropertySequence>(collectionName);
    }

    public async Task<PropertySequence> GetSequenceAndUpdate(FilterDefinition<PropertySequence> filterDefinition)
    {
        FindOneAndUpdateOptions<PropertySequence, PropertySequence> findOneAndUpdateOptions =
            new FindOneAndUpdateOptions<PropertySequence, PropertySequence>
                {IsUpsert = true, ReturnDocument = ReturnDocument.After};
        UpdateDefinition<PropertySequence> updateDefinition = Builders<PropertySequence>.Update.Inc(sequence => sequence.SequenceValue, 1);
        return await _collection.FindOneAndUpdateAsync(filterDefinition, updateDefinition, findOneAndUpdateOptions);
    }
}