using System;
using System.Threading.Tasks;
using Application.Common.Interfaces;

namespace Infrastructure.Persistence
{
    public abstract class CouchbaseRepository<TEntity> : ICouchbaseRepository<TEntity> where TEntity : class
    {
        protected readonly ICouchbaseContext _couchbaseContext;
        protected readonly string _entity;

        protected CouchbaseRepository(ICouchbaseContext couchbaseContext)
        {
            _couchbaseContext = couchbaseContext;
            _entity = typeof(TEntity).Name;
        }

        async public Task<TEntity> GetDocument(string id)
        {
            try
            {
                var result = await _couchbaseContext.Collection
                    .GetAsync($"{_entity}-{id}")
                    .ConfigureAwait(false);

                return result.ContentAs<TEntity>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        async public Task<TEntity> UpsertDocument(string id, TEntity entity)
        {
            await _couchbaseContext.Collection
                .UpsertAsync($"{_entity}-{id}", entity)
                .ConfigureAwait(false);

            return await GetDocument(id).ConfigureAwait(false);
        }
    }
}
