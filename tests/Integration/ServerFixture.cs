using System.IO;
using System.Net.Http;
using Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Xunit;
using Xunit.Abstractions;

namespace Integration
{
    public class ServerFixture
    {
        public readonly HttpClient client;

        public ServerFixture()
        {
            TestServer testServer = new TestServer(new WebHostBuilder()
                .ConfigureAppConfiguration((context, builder) => {
                    string rootDir = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent}/src/config";
                    builder.SetBasePath(rootDir);
                    builder.AddJsonFile("appsettings.Testing.json");
                })
                .UseStartup<Startup>());

            client = testServer.CreateClient();
        }
    }

    [CollectionDefinition("Server collection")]
    public class ServerCollection : ICollectionFixture<ServerFixture>
    {
    }
}