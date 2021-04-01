using System.IO;
using System.Net.Http;
using Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Xunit;
using Bogus;
using Domain.Entities;
using System.Collections.Generic;

namespace Integration
{
    public class ServerFixture
    {
        public readonly HttpClient client;
        public TestWorldRepository worldRepository;
        public Faker<World> worldFaker;

        public ServerFixture()
        {
            TestServer testServer = new TestServer(new WebHostBuilder()
                .ConfigureAppConfiguration((context, builder) => {
                    string rootDir = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent}/src/config";
                    builder.SetBasePath(rootDir);
                    builder.AddJsonFile("appsettings.Testing.json");
                })
                .UseStartup<Startup>());

            worldFaker = CreateWorldFaker();
            client = testServer.CreateClient();
            worldRepository = new CouchbaseFixture().worldRepository;
        }

        private Faker<World> CreateWorldFaker()
        {
            return new Faker<World>()
            .RuleFor(p => p.Id, f => f.Random.Guid().ToString())
            .RuleFor(p => p.Name, f => f.PickRandom<string>(new List<string>{"Mercury", "Jupiter", "Mars","Earth", "Saturn"}))
            .RuleFor(p => p.HasLife, f => f.Random.Bool())
            .RuleFor(p => p.Entity, "World");
        }
    }

    [CollectionDefinition("Server collection")]
    public class ServerCollection : ICollectionFixture<ServerFixture>
    {
    }
}