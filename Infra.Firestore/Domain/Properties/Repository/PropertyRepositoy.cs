using Infra.Firestore.Commons.Connection;
using Infra.Firestore.Commons.Repository;
using Infra.Firestore.Domain.Properties.Model;

namespace Infra.Firestore.Domain.Properties.Repository;

public class PropertyRepository : Repository<PropertyEntity>
{
    public PropertyRepository(IConnectionFactory connectionFactory, string database, string collection) : base(connectionFactory, database, collection)
    {
    }
}