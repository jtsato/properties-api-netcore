using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.Properties.Gateways;
using Core.Domains.Properties.Models;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.Properties.Mapper;
using Infra.MongoDB.Domains.Properties.Model;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.Properties.Providers;

public sealed class RegisterPropertyProvider : IRegisterPropertyGateway
{
    private const string KeyField = "propertyId";

    private readonly IRepository<PropertyEntity> _propertyRepository;
    private readonly ISequenceRepository<PropertySequence> _propertySequenceRepository;

    public RegisterPropertyProvider(
        IRepository<PropertyEntity> propertyRepository,
        ISequenceRepository<PropertySequence> propertySequenceRepository
    )
    {
        _propertyRepository = ArgumentValidator.CheckNull(propertyRepository, nameof(propertyRepository));
        _propertySequenceRepository = ArgumentValidator.CheckNull(propertySequenceRepository, nameof(propertySequenceRepository));
    }

    public async Task<Property> ExecuteAsync(Property property)
    {
        PropertyEntity entity = property.Map();

        await IncrementId(entity);

        PropertyEntity propertyEntity = await _propertyRepository.SaveAsync(entity);

        return propertyEntity.Map();
    }

    private async Task IncrementId(Entity entity)
    {
        FilterDefinition<PropertySequence> filter = Builders<PropertySequence>.Filter.Eq(sequence => sequence.SequenceName, KeyField);
        PropertySequence propertySequence = await _propertySequenceRepository.GetSequenceAndUpdate(filter);
        entity.Id = propertySequence.SequenceValue;
    }
}