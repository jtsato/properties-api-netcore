using Google.Cloud.Firestore;

namespace Infra.Firestore.Commons.Connection;

public class ConnectionFactory: IConnectionFactory
{
    private readonly string _connectionString;
    
    public ConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public FirestoreDb GetDatabase(string database)
    {
        FirestoreDb firestoreDb = new FirestoreDbBuilder
        {
            JsonCredentials = _connectionString,
            ProjectId = database
        }.Build();
        
        return firestoreDb;
    }
}

