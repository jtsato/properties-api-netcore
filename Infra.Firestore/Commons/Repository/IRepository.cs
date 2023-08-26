using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Commons;
using Core.Commons.Paging;
using Google.Cloud.Firestore;

namespace Infra.Firestore.Commons.Repository;

public interface IRepository<T>
{
    FirestoreDb GetDatabase();
    
    Query GetBaseQuery(Filter filter);
    
    Task<Optional<T>> FindOneAsync(Filter filter);
    
    Task<Page<T>> FindAllAsync(IEnumerable<Filter> filters, PageRequest pageRequest);
    
    Task<Page<T>> FindAllAsync(Query baseQuery, PageRequest pageRequest);
}