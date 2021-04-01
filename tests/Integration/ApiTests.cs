using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Integration
{
    [Collection("Server collection")]
    public class ApiTest
    {
        private readonly HttpClient _testClient;

        private readonly TestWorldRepository _testWorldRepository;
        private Faker<World> _worldFaker;

        public ApiTest(ServerFixture serverFixture)
        {
            _testClient = serverFixture.client;
            _testWorldRepository = serverFixture.worldRepository;
            _worldFaker = serverFixture.worldFaker;
        }

        [Fact]
        async Task Get_Worlds_ShouldReturn_Worlds()
        {
            await _testWorldRepository.DeleteDocuments();
            await _testWorldRepository.InsertDocument(_worldFaker.Generate(1)[0]);

            HttpResponseMessage response = await _testClient.GetAsync("/api/worlds");
            string responseString = await response.Content.ReadAsStringAsync();

            World Worlds = JsonSerializer.Deserialize<World>(responseString);
            Worlds.Should().NotBeNull();
        }

        async Task Get_WorldById_ShouldReturn_A_World()
        {
            await _testWorldRepository.DeleteDocuments();
            await _testWorldRepository.InsertDocument(_worldFaker.Generate(1)[0]);

            HttpResponseMessage response = await _testClient.GetAsync("/api/worlds?id=dsfsdfsdfsd-ded");
            string responseString = await response.Content.ReadAsStringAsync();

            World Worlds = JsonSerializer.Deserialize<World>(responseString);
            Worlds.Should().NotBeNull();
        }

        [Fact]
        async Task Post_World_returns_World_Object()
        {
            await _testWorldRepository.DeleteDocuments();
            World worldToSave = _worldFaker.Generate(1)[0];

            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(worldToSave), Encoding.UTF8, "application/json");
            HttpResponseMessage response = _testClient.PostAsync("/api/worlds", httpContent).Result;

            string responseString = await response.Content.ReadAsStringAsync();
            World savedWorld = JsonSerializer.Deserialize<World>(responseString);
            savedWorld.Should().NotBeNull();
        }
    }
}