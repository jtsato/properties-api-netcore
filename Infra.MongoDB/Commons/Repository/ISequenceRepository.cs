using MongoDB.Driver;

namespace Infra.MongoDB.Commons.Repository;

public interface ISequenceRepository<T> where T : ISequence
{
    T GetSequenceAndUpdate(FilterDefinition<T> filterDefinition);
}