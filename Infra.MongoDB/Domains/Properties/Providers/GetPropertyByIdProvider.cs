using System;
using System.Threading.Tasks;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;
using Core.Domains.Properties.UseCases;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.Properties.Mapper;
using Infra.MongoDB.Domains.Properties.Model;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.Properties.Providers;

public class GetPropertyByIdProvider : IGetPropertyByIdGateway
{
    private readonly IRepository<PropertyEntity> _repository;
    
    public GetPropertyByIdProvider(IRepository<PropertyEntity> repository)
    {
        _repository = repository;
    }
    
    public async Task<Core.Commons.Optional<Property>> ExecuteAsync(GetPropertyByIdQuery query)
    {
        FilterDefinition<PropertyEntity> filterDefinition = GetFilterDefinition(Convert.ToInt64(query.Id));
        Core.Commons.Optional<PropertyEntity> optional = await _repository.FindOneAsync(filterDefinition);
        return optional.Map(PropertyMapper.Of);
    }
    
    private static FilterDefinition<PropertyEntity> GetFilterDefinition(long id)
    {
        return Builders<PropertyEntity>.Filter.Eq(document => document.Id, id);
    }
}