using Couchbase;
using Couchbase.KeyValue;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Common.Interfaces;
namespace Integration
{
    public class TestEntityRepository<TEntity> where TEntity: AuditableEntity, ICouchbaseEntity
    {
        private readonly TestCouchbaseContext _couchbaseContext;
        private readonly string _entity;
        public TestEntityRepository()
        {
            _couchbaseContext = new TestCouchbaseContext();
            _entity = typeof(TEntity).Name;
        }
        async public Task<TEntity> FindOneDocument(string id)
        {
            var start = DateTime.Now;

            var result = await _couchbaseContext.Collection
                .GetAsync($"{_entity}-{id}");

            return result.ContentAs<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> FindAllDocuments(int limit = 20, int offset = 0)
        {
            var start = DateTime.Now;

            string query = "SELECT * from $bucket where entity = $entityName LIMIT $limit OFFSET $offset";
            var cbResults = await _couchbaseContext.Bucket.Cluster
                .QueryAsync<dynamic>(query,
                                     options => options
                                        .Parameter("bucket", "World")
                                        .Parameter("entityName", _entity)
                                        .Parameter("limit", limit)
                                        .Parameter("offset", offset));

            var results = new List<TEntity> { };

            await foreach (var result in cbResults)
            {
                results.Add(result[_entity].ToObject<TEntity>());
            }

            return results;

        }

        public async Task<IAsyncEnumerable<int>> Count()
        {
            var start = DateTime.Now;

            var cluster = _couchbaseContext.Bucket.Cluster;
            string query = "SELECT RAW count(*) from $bucket where entity = $entityName";
            var results = await cluster.QueryAsync<int>(query, options => {
                options
                .Parameter("bucket", "WorldTest")
                .Parameter("entityName", _entity);
            });

            return results.Rows;
        }

        public async Task<TEntity> InsertDocument(TEntity entity)
        {
            var start = DateTime.Now;

            var id = entity.Id == null ? Guid.NewGuid().ToString() : entity.Id;
            entity.Entity = _entity;
            entity.Id = id;
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;

            await _couchbaseContext.Collection.InsertAsync($"{_entity}-{id}", entity);

            return await FindOneDocument(id);
        }

        public async Task DeleteDocuments()
        {

            var cluster = _couchbaseContext.Bucket.Cluster;
            string query = "DELETE FROM WorldTest where entity='World'";
            var results = await cluster.QueryAsync<string>(query);
        }

        public async Task<TEntity> InsertSubDocument(string documentId, string subDocumentId, dynamic subDocumentValue)
        {
            var start = DateTime.Now;

            await _couchbaseContext.Collection.MutateInAsync(
                documentId,
                specs => specs.Insert(subDocumentId, subDocumentValue));

            return await UpsertDocument(documentId, await FindOneDocument(documentId));
        }

        public async Task<TEntity> UpsertSubDocument(string documentId, string subDocumentId, dynamic subDocumentValue)
        {
            var start = DateTime.Now;

            await _couchbaseContext.Collection.MutateInAsync(
                documentId,
                specs => specs.Upsert(subDocumentId, subDocumentValue));

            return await UpsertDocument(documentId, await FindOneDocument(documentId));
        }

        public async Task<TEntity> UpsertDocument(string id, TEntity entity)
        {
            var start = DateTime.Now;

            entity.Entity = _entity;

            await _couchbaseContext.Collection
                .UpsertAsync($"{_entity}-{id}", entity);

            return await FindOneDocument(id);
        }

        public async Task<string> RemoveDocument(string id, TEntity entity)
        {
            var start = DateTime.Now;

            entity.Entity = _entity;

            await _couchbaseContext.Collection
                .RemoveAsync($"{_entity}-{id}");

            return id;
        }
    }
}
