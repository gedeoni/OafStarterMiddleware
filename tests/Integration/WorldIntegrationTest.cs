using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Integration
{
    [Collection("Server collection")]
    public class IntegrationTest
    {
        private readonly HttpClient _testClient;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IWorldRepository _worldRepo;

        public IntegrationTest(ServerFixture serverFixture, IWorldRepository worldRepo)
        {
            _testClient = serverFixture.client;
            _worldRepo = worldRepo;
        }

        [Fact]
        async Task Get_World_Returns_Success()
        {
            var response = await _testClient.GetAsync("/api/worlds");
            Action checkResponse = () => response.EnsureSuccessStatusCode();
            checkResponse.Should().NotThrow<HttpRequestException>();
        }

        [Fact]
        async Task Post_World_returns_World_Object()
        {
            World worldToSave = new World { Name = "TestWorld", HasLife = true };
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(worldToSave), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _testClient.PostAsync("/api/worlds", httpContent);
            string responseString = await response.Content.ReadAsStringAsync();
            World savedWorld = JsonSerializer.Deserialize<World>(responseString);
            savedWorld.Should().NotBeNull();
        }
    }
}