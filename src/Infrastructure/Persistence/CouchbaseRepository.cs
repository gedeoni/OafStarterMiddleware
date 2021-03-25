using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Couchbase;
using Couchbase.KeyValue;
using Couchbase.Query;
using Domain.Common.Interfaces;

namespace Infrastructure.Persistence
{
    public abstract class CouchbaseRepository<TEntity> : ICouchbaseRepository<TEntity> where TEntity : AuditableEntity, ICouchbaseEntity
    {
        protected readonly ICouchbaseContext _couchbaseContext;
        protected readonly string _entity;

        protected CouchbaseRepository(ICouchbaseContext couchbaseContext)
        {
            _couchbaseContext = couchbaseContext;
            _entity = typeof(TEntity).Name;
        }

        async public Task<TEntity> FindOneDocument(string id)
        {
            var result = await _couchbaseContext.Collection
                .GetAsync($"{_entity}-{id}");

            return result.ContentAs<TEntity>();
        }

        //TODO: RawQuery
        //TODO: Subdocument with Custom Operation (for array and mulitple mutations etc)
        //TODO: Bulk Create
        //TODO: Replace Document
        //TODO: Remove SubDocument
        //TODO: Replace SubDocument

        public async Task<IEnumerable<TEntity>> FindAllDocuments(int limit = 20, int offset = 0)
        {
            string query = "SELECT * from World where entity = $entityName LIMIT $limit OFFSET $offset";
            var results = await _couchbaseContext.Bucket.Cluster
                .QueryAsync<dynamic>(query,
                                     options => options
                                        .Parameter("entityName", _entity)
                                        .Parameter("limit", limit)
                                        .Parameter("offset", offset));

            return (IEnumerable<TEntity>)results.Rows;
        }

        public async Task<IAsyncEnumerable<int>> Count()
        {
            var cluster = _couchbaseContext.Bucket.Cluster;
            string query = "SELECT RAW count(*) from World where entity = $entityName";
            var results = await cluster.QueryAsync<int>(query, options => {
                options.Parameter("entityName", _entity);
            });

            return results.Rows;
        }

        public async Task<TEntity> InsertDocument(TEntity entity)
        {
            var id = entity.Id == null ? Guid.NewGuid().ToString() : entity.Id;
            entity.Entity = _entity;
            entity.CreatedAt = DateTime.Now;

            await _couchbaseContext.Collection.InsertAsync($"{_entity}-{id}", entity);

            return await FindOneDocument(id);
        }

        public async Task<TEntity> InsertSubDocument(string documentId, string subDocumentId, dynamic subDocumentValue)
        {
            await _couchbaseContext.Collection.MutateInAsync(
                documentId,
                specs => specs.Insert(subDocumentId, subDocumentValue));

            return await UpsertDocument(documentId, await FindOneDocument(documentId));
        }

        public async Task<TEntity> UpsertSubDocument(string documentId, string subDocumentId, dynamic subDocumentValue)
        {
            await _couchbaseContext.Collection.MutateInAsync(
                documentId,
                specs => specs.Upsert(subDocumentId, subDocumentValue));

            return await UpsertDocument(documentId, await FindOneDocument(documentId));
        }

        public async Task<TEntity> UpsertDocument(string id, TEntity entity)
        {
            entity.Entity = _entity;

            await _couchbaseContext.Collection
                .UpsertAsync($"{_entity}-{id}", entity);

            return await FindOneDocument(id);
        }

        public async Task<string> RemoveDocument(string id, TEntity entity)
        {
            entity.Entity = _entity;

            await _couchbaseContext.Collection
                .RemoveAsync($"{_entity}-{id}");

            return id;
        }
    }
}
