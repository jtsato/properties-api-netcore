using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Commons;
using Core.Commons.Paging;
using Google.Cloud.Firestore;
using Infra.Firestore.Commons.Connection;

namespace Infra.Firestore.Commons.Repository;

public class Repository<T> : IRepository<T>
{
    private readonly FirestoreDb _firestoreDb;
    private readonly string _collection;

    protected Repository(IConnectionFactory connectionFactory, string database, string collection)
    {
        _firestoreDb = connectionFactory.GetDatabase(database);
        _collection = collection;
    }

    public FirestoreDb GetDatabase()
    {
        return _firestoreDb;
    }

    public async Task<Optional<T>> FindOneAsync(Filter filter)
    {
        CollectionReference collectionReference = _firestoreDb.Collection(_collection);
        Query query = collectionReference.Where(filter);

        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        if (snapshot.Count == 0)
        {
            return Optional<T>.Empty();
        }

        DocumentSnapshot document = snapshot.Documents[0];
        return Optional<T>.Of(document.ConvertTo<T>());
    }

    public async Task<Page<T>> FindAllAsync(IEnumerable<Filter> filters, PageRequest pageRequest)
    {
        CollectionReference collectionReference = _firestoreDb.Collection(_collection);
        Filter where = filters.Aggregate<Filter, Filter>(null, (current, filter) => current == null ? filter : Filter.And(current, filter));
        Query baseQuery = where != null ? collectionReference.Where(where) : collectionReference;
        
        return await FindAllAsync(baseQuery, pageRequest);
    }
    
    public Query GetBaseQuery(Filter filter)
    {
        CollectionReference collectionReference = _firestoreDb.Collection(_collection);
        return filter != null ? collectionReference.Where(filter) : collectionReference;
    }

    public async Task<Page<T>> FindAllAsync(Query baseQuery, PageRequest pageRequest)
    {
        int offset = (pageRequest.PageNumber) * pageRequest.PageSize;

        AggregateQuery aggregateQuery = baseQuery.Count();
        AggregateQuerySnapshot countQuerySnapshot = await aggregateQuery.GetSnapshotAsync();

        int totalOfElements = Convert.ToInt32(countQuerySnapshot.Count);

        Query query = baseQuery.Offset(offset).Limit(pageRequest.PageSize);

        query = pageRequest.Sort.GetOrders()
            .Aggregate(query, (current, order) => order.Direction == Direction.Asc
                ? current.OrderBy(ToLowerCamelCase(order.Property))
                : current.OrderByDescending(ToLowerCamelCase(order.Property)));

        // TODO: Fix Order By

        QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
        int numberOfElements = querySnapshot.Count;
        int totalPages = (int) Math.Ceiling((double) totalOfElements / pageRequest.PageSize) - 1;

        Pageable pageable = new Pageable(
            pageRequest.PageNumber,
            pageRequest.PageSize,
            numberOfElements,
            totalOfElements,
            totalPages
        );

        List<T> content = querySnapshot.Documents.Select(document => document.ConvertTo<T>()).ToList();

        return new Page<T>(content, pageable);
    }

    private static string ToLowerCamelCase(string value)
    {
        return char.ToLowerInvariant(value[0]) + value[1..];
    }
}