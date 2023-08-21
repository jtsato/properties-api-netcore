using System;
using System.Collections.Generic;
using System.Linq;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.Admin.V1;
using Google.LongRunning;
using Infra.Firestore.Commons.Connection;
using Infra.Firestore.Commons.Repository;
using Infra.Firestore.Domain.Properties.Model;
using Index = Google.Cloud.Firestore.Admin.V1.Index;
using IndexField = Google.Cloud.Firestore.Admin.V1.Index.Types.IndexField;
using Order = Google.Cloud.Firestore.Admin.V1.Index.Types.IndexField.Types.Order;
using ApiScope = Google.Cloud.Firestore.Admin.V1.Index.Types.ApiScope;
using State = Google.Cloud.Firestore.Admin.V1.Index.Types.State;
using QueryScope = Google.Cloud.Firestore.Admin.V1.Index.Types.QueryScope;

namespace Infra.Firestore.Domain.Properties.Repository;

public class PropertyRepository : Repository<PropertyEntity>
{
    public PropertyRepository(IConnectionFactory connectionFactory, string database, string collection) : base(connectionFactory, database, collection)
    {
        // TODO: Create indexes in a better way
        // CreateIndex(connectionFactory.GetDatabase(database), collection, "tenant_type_transaction", new List<string> {"tenantId", "type", "transaction"});
    }

    private static void CreateIndex(FirestoreDb db, string collectionName, string identifier, List<string> fieldPaths)
    {
        CollectionReference collectionRef = db.Collection(collectionName);

        string indexName = $"idx_{collectionName}_{identifier}";

        List<IndexField> indexFields = fieldPaths.Select(fieldPath => new IndexField {FieldPath = fieldPath, Order = Order.Ascending}).ToList();

        CreateIndexRequest createIndexRequest = new CreateIndexRequest
        {
            Parent = collectionRef.Path,
            Index = new Index
            {
                Fields = {indexFields},
                QueryScope = QueryScope.Collection,
                State = State.Creating,
                Name = indexName,
                ApiScope = ApiScope.DatastoreModeApi,
            },
        };

        FirestoreAdminClient adminClient = FirestoreAdminClient.Create();
        Operation<Index, IndexOperationMetadata> operation = adminClient.CreateIndex(createIndexRequest);
        Index index = operation.PollUntilCompleted().Result;

        string fieldNames = string.Join(',', index.Fields.Select(element => element.FieldPath));
        Console.WriteLine($"Index '{indexName}' created for collection '{collectionName}' on field(s) '{fieldNames}'.");
    }
}