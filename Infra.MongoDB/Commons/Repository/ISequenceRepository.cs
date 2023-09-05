using System.Threading.Tasks;
using Infra.MongoDB.Domains.Properties.Model;
using MongoDB.Driver;

namespace Infra.MongoDB.Commons.Repository;

public interface ISequenceRepository<T> where T : ISequence
{
    Task<PropertySequence> GetSequenceAndUpdate(FilterDefinition<T> filterDefinition);
}