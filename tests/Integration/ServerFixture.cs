using System.IO;
using Xunit;
using Api;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Xunit.Abstractions;

namespace Integration
{
    public class ServerFixture
    {
        public readonly HttpClient client;

        public ServerFixture()
        {
            TestServer testServer = new TestServer(new WebHostBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    string rootDir =$"{ Directory.GetParent(Directory.GetParent( Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName).FullName}/testconfig";
                    builder.SetBasePath(rootDir);
                    builder.AddJsonFile("appsettings.Test.json");
                })
                .UseStartup<Startup>());

            client = testServer.CreateClient();
        }
    }

    [CollectionDefinition("Server collection")]
    public class ServerCollection: ICollectionFixture<ServerFixture>
    {
    }
}