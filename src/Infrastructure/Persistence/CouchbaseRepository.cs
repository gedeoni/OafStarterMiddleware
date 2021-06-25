using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Couchbase;
using Couchbase.KeyValue;
using Couchbase.Query;
using Domain.Common.Attributes;
using Domain.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence
{
    public abstract class CouchbaseRepository<TEntity> : ICouchbaseRepository<TEntity> where TEntity : AuditableEntity, ICouchbaseEntity
    {
        protected readonly ICouchbaseContext _couchbaseContext;
        protected readonly string _entity;
        protected readonly string _bucketName;
        private readonly IAttributesProcessor _attrProcessor;
        protected readonly ILogger<CouchbaseRepository<TEntity>> _logger;

        protected CouchbaseRepository(
            ICouchbaseContext couchbaseContext,
            ILogger<CouchbaseRepository<TEntity>> logger,
            IConfiguration config,
            IAttributesProcessor attrProcessor
        )
        {
            _couchbaseContext = couchbaseContext;
            _logger = logger;
            _bucketName = config.GetSection("Couchbase:BucketName").Value;
            EntityNameAttribute _entityNameAttr = _attrProcessor
                .GetAttributeInstance<EntityNameAttribute>(typeof(TEntity));
            _entity = _entityNameAttr != null
                ? _entityNameAttr.Name
                : typeof(TEntity).Name;
        }

        async public Task<TEntity> FindOneDocument(string id)
        {
            var start = DateTime.Now;

            var result = await _couchbaseContext.Collection
                .GetAsync($"{_entity}-{id}");

            _logger.LogInformation(
                "CouchbaseOperation: Executed FindOne operation on {Entity} with {Id} in {Elapsed:000}ms",
                _entity,
                id,
                DateTime.Now.Subtract(start).Milliseconds);

            return result.ContentAs<TEntity>();
        }

        //TODO: RawQuery
        //TODO: Subdocument with Custom Operation (for array and mulitple mutations etc)
        //TODO: Bulk Create
        //TODO: Replace Document
        //TODO: Remove SubDocument
        //TODO: Replace SubDocument
        //TODO: Dynamic Where Clause

        public async Task<IEnumerable<TEntity>> FindAllDocuments(int limit = 20, int offset = 0)
        {
            var start = DateTime.Now;

            string query = $"SELECT * from `{_bucketName}` where entity = $entityName LIMIT $limit OFFSET $offset";

            var cbResults = await _couchbaseContext.Bucket.Cluster
                .QueryAsync<dynamic>(query,
                                     options => options
                                        .Parameter("entityName", _entity)
                                        .Parameter("limit", limit)
                                        .Parameter("offset", offset));

            _logger.LogInformation(
                "CouchbaseOperation: Executed FindAll operation on {Entity} in {Elapsed:000}ms",
                _entity,
                DateTime.Now.Subtract(start).Milliseconds);

            var results = new List<TEntity> { };

            await foreach (var result in cbResults)
            {
                var parsedResult = result[_bucketName].ToObject<TEntity>();
                results.Add(parsedResult);
            }

            return results;

        }

        public async Task<int> Count()
        {
            var start = DateTime.Now;

            var cluster = _couchbaseContext.Bucket.Cluster;
            string query = $"SELECT RAW count(*) from `{_bucketName}` where entity = $entityName";
            var couchbaseResults = await cluster.QueryAsync<int>(query, options => {
                options
                .Parameter("entityName", _entity);
            });

            _logger.LogInformation(
                "CouchbaseOperation: Executed Count operation on {Entity} in {Elapsed:000}ms",
                _entity,
                DateTime.Now.Subtract(start).Milliseconds);

            var results = await couchbaseResults.Rows.ToListAsync().AsTask();
            return results[0];
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

            _logger.LogInformation(
                "CouchbaseOperation: Executed Insert operation on {Entity} with {Id} in {Elapsed:000}ms",
                entity,
                entity.Id,
                DateTime.Now.Subtract(start).Milliseconds);

            return await FindOneDocument(id);
        }

        public async Task<TEntity> InsertSubDocument(string documentId, string subDocumentId, dynamic subDocumentValue)
        {
            var start = DateTime.Now;

            await _couchbaseContext.Collection.MutateInAsync(
                documentId,
                specs => specs.Insert(subDocumentId, subDocumentValue));

            _logger.LogInformation(
                "CouchbaseOperation: Executed InsertSubDocument operation on {Entity} with {Id} and {SubDocumentId} in {Elapsed:000}ms",
                _entity,
                documentId,
                subDocumentId,
                DateTime.Now.Subtract(start).Milliseconds);

            return await UpsertDocument(documentId, await FindOneDocument(documentId));
        }

        public async Task<TEntity> UpsertSubDocument(string documentId, string subDocumentId, dynamic subDocumentValue)
        {
            var start = DateTime.Now;

            await _couchbaseContext.Collection.MutateInAsync(
                documentId,
                specs => specs.Upsert(subDocumentId, subDocumentValue));

            _logger.LogInformation(
                "CouchbaseOperation: Executed UpsertSubdocument operation on {Entity} with {Id} and {SubDocumentId} in {Elapsed:000}ms",
                _entity,
                documentId,
                subDocumentId,
                DateTime.Now.Subtract(start).Milliseconds);

            return await UpsertDocument(documentId, await FindOneDocument(documentId));
        }

        public async Task<TEntity> UpsertDocument(string id, TEntity entity)
        {
            var start = DateTime.Now;

            entity.Entity = _entity;

            await _couchbaseContext.Collection
                .UpsertAsync($"{_entity}-{id}", entity);

            _logger.LogInformation(
                "CouchbaseOperation: Executed Upsert operation on {Entity} with {Id} in {Elapsed:000}ms",
                _entity,
                id,
                DateTime.Now.Subtract(start).Milliseconds);

            return await FindOneDocument(id);
        }

        public async Task<string> RemoveDocument(string id)
        {
            var start = DateTime.Now;

            await _couchbaseContext.Collection
                .RemoveAsync($"{_entity}-{id}");

            _logger.LogInformation(
                "CouchbaseOperation: Executed Remove operation on {Entity} with {Id} in {Seconds}",
                _entity,
                id,
                DateTime.Now.Subtract(start).Milliseconds);

            return id;
        }
    }
}
