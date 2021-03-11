using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ICouchbaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetDocument(string id);
        Task<TEntity> UpsertDocument(string id, TEntity entity);
    }
}
