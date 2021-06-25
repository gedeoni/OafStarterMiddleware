using System;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Common.Extensions;
using Domain.Common.Interfaces;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace IntegrationTests
{
    using static Testing;

    public sealed class TestEntity : CouchbaseEntity<TestEntity>, IAuditableEntity
    {
        public static readonly string ENTITY_NAME = "test-entity";
        public DateTime? CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CouchbaseRepositoryTests
    {
        private IWorldRepository _worldRepo;
        private ICouchbaseContext _couchbaseContext;
        private string _bucketName;

        [SetUp]
        public async Task ResetDatabase()
        {

            using var scope = serviceScopeFactory.CreateScope();
            _worldRepo = scope.ServiceProvider.GetService<IWorldRepository>();
            _couchbaseContext = scope.ServiceProvider.GetService<ICouchbaseContext>();
            var couchbaseOptions = _configuration.GetSection("Couchbase").Get<CouchbaseConfig>();
            _bucketName = couchbaseOptions.BucketName;
            await Task.Delay(100); //Delay after insertion to do some async magic
            await FlushBucket();
        }

        [Test]
        public async Task TestThatCountWorks()
        {
            //Arrange - Use the Context to Create data in the Datbase
            var id = Guid.NewGuid().ToString();
            var fakeWorld = new World() { Name = "Mars", HasLife = true, Id = id };
            var results = await _couchbaseContext.Collection.InsertAsync(id, fakeWorld);
            await Task.Delay(100); //Delay after insertion to do some async magic
            //Act
            var count = await _worldRepo.Count();

            //Assert
            count.Should().Be(1);
        }

        [Test]
        public async Task TestThatFindOneDocumentWorks()
        {
            //Arrange - Use the Context to Create data in the Database
            var id = Guid.NewGuid().ToString();
            var fakeWorld = new World() { Name = "Mars", HasLife = true, Id = id };
            var results = await _couchbaseContext.Collection.InsertAsync($"{fakeWorld.Entity}-{id}", fakeWorld);

            //Act
            var expected = await _worldRepo.FindOneDocument(id);

            //Assert
            expected.Id.Should().Be(fakeWorld.Id);
        }

        [Test]
        public async Task TestThatInsertDocumentWorks()
        {
            //Arrange - Use the Context to Create data in the Database
            var fakeWorld = new World() { Name = "Mars", HasLife = true };

            //Act
            var expected = await _worldRepo.InsertDocument(fakeWorld);

            //Assert
            expected.Should().BeOfType<World>();
            expected.Name.Should().Be(fakeWorld.Name);
            expected.Entity.Should().Be(fakeWorld.Entity);
        }

        [Test]
        public async Task TestThatRemoveDocumentWorks()
        {
            //Arrange - Use the Context to Create data in the Database
            var id = Guid.NewGuid().ToString();
            var fakeWorld = new World() { Name = "Mars", HasLife = true, Id = id };
            var results = await _couchbaseContext.Collection.InsertAsync($"{fakeWorld.Entity}-{id}", fakeWorld);
            await Task.Delay(100); //Delay after insertion to do some async magic

            //Act
            var expected = await _worldRepo.RemoveDocument(id);

            //Assert
            expected.Should().Be(id);
        }

        [Test]
        public async Task TestThatUpsertDocumentWorks()
        {
            //Arrange - Use the Context to Create data in the Database
            var id = Guid.NewGuid().ToString();
            var fakeWorld = new World() { Name = "Mars", HasLife = true, Id = id };
            var results = await _couchbaseContext.Collection.InsertAsync($"{fakeWorld.Entity}-{id}", fakeWorld);
            fakeWorld.Name = "Jupiter";

            //Act
            var expected = await _worldRepo.UpsertDocument(id, fakeWorld);

            //Assert
            expected.Name.Should().Be("Jupiter");
        }

        [Test]
        public async Task TestThatPreferredEntityNameIsSet()
        {
            //Arrange - Use the Context to Create data in the Database
            var id = Guid.NewGuid().ToString();
            var fakeEntity = new TestEntity() { Id = id };
            var results = await _couchbaseContext.Collection.InsertAsync($"{fakeEntity.Entity}-{id}", fakeEntity);

            //Act
            var result = await _couchbaseContext.Collection
                .GetAsync($"{typeof(TestEntity).GetEntityName()}-{id}");

            //Assert
            var expected = result.ContentAs<TestEntity>();
            expected.Entity.Should().Be(TestEntity.ENTITY_NAME);
        }
    }
}