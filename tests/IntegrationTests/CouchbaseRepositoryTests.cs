using System;
namespace IntegrationTests
{
    using System;
    using Application.Common.Interfaces;
    using Domain.Entities;
    using FluentAssertions;
    using Infrastructure.Persistence;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    namespace IntegrationTests
    {
        public class CouchbaseRepositoryTests : IClassFixture<ServerFixture>
        {
            private readonly IWorldRepository _worldRepo;
            private readonly ServerFixture _fixture;
            private readonly ICouchbaseContext _couchbaseContext;
            private readonly string _bucketName;

            public CouchbaseRepositoryTests(ServerFixture fixture)
            {
                using var scope = ServerFixture.serviceScopeFactory.CreateScope();
                _worldRepo = scope.ServiceProvider.GetService<IWorldRepository>();
                _fixture = fixture;

                _couchbaseContext = scope.ServiceProvider.GetService<ICouchbaseContext>();
                var couchbaseOptions = fixture._configuration.GetSection("Couchbase").Get<CouchbaseConfig>();

                _bucketName = couchbaseOptions.BucketName;
            }

            [Fact]
            async void TestThatCountMethodWorks()
            {
                //Arrange - Use the Context to Create data in the Datbase
                var id = Guid.NewGuid().ToString();
                var fakeWorld = new World() { Name = "Mars", HasLife = true, Id = id, Entity = "World" };
                var results = await _couchbaseContext.Collection.InsertAsync(id, fakeWorld);

                //Act
                var count = await _worldRepo.Count();

                //Assert
                count.Should().Be(1);
            }
        }
    }

}