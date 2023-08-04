using Google.Cloud.Firestore;

namespace Infra.Firestore.Commons.Connection;

public interface IConnectionFactory
{
    FirestoreDb GetDatabase(string database);
}
