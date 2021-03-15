using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using System;
using Domain.Entities;
using System.Text.Json;
using System.Text;

namespace Integration
{
    [Collection("Server collection")]
    public class Integration
    {
        private readonly HttpClient testClient;
        private readonly ITestOutputHelper _testOutputHelper;
        public Integration(ServerFixture serverFixture)
        {
            testClient = serverFixture.client;
        }

        [Fact]
        async Task Get_World_Returns_Success()
        {
            var response = await testClient.GetAsync("/api/world");
            Action checkResponse = () => response.EnsureSuccessStatusCode();
            checkResponse.Should().NotThrow<HttpRequestException>();
        }

        [Fact]
        async Task Post_World_returns_WorldObject()
        {
            World worldToSave = new World {Name="TestWorld", HasLife=true};
            string url = "/api/world";
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(worldToSave),Encoding.UTF8, "application/json");
            HttpResponseMessage response = await testClient.PostAsync(url, httpContent);
            string responseString = await response.Content.ReadAsStringAsync();
            World savedWorld = JsonSerializer.Deserialize<World>(responseString);
            savedWorld.Should().NotBeNull();
        }
    }
}