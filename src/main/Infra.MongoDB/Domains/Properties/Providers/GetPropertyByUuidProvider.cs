using System.Threading.Tasks;
using Core.Domains.Properties.Gateways;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.Properties.Mapper;
using Infra.MongoDB.Domains.Properties.Model;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.Properties.Providers;

public class GetPropertyByUuidProvider : IGetPropertyByUuidGateway
{
    private readonly IRepository<PropertyEntity> _repository;

    public GetPropertyByUuidProvider(IRepository<PropertyEntity> repository)
    {
        _repository = repository;
    }

    public async Task<Core.Commons.Optional<Property>> ExecuteAsync(GetPropertyByUuidQuery query)
    {
        FilterDefinition<PropertyEntity> filterDefinition = GetFilterDefinition(query.Uuid);
        Core.Commons.Optional<PropertyEntity> optional = await _repository.FindOneAsync(filterDefinition);
        return optional.Map(PropertyMapper.Map);
    }

    private static FilterDefinition<PropertyEntity> GetFilterDefinition(string uuid)
    {
        return Builders<PropertyEntity>.Filter.Eq(document => document.Uuid, uuid);
    }
}