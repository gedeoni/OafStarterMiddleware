using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace IntegrationTests
{
    using static Testing;

    public class WorldAPITests
    {
        private IWorldRepository _worldRepo;
        private HttpClient _testClient;

        [SetUp]
        public async Task ResetDatabase()
        {
            using var scope = serviceScopeFactory.CreateScope();
            _worldRepo = scope.ServiceProvider.GetService<IWorldRepository>();
            _testClient = client;
            await Task.Delay(100);
            await FlushBucket();
        }

        [Test]
        public async Task TestThatGetWorldsEndpointWorks()
        {
            //Arrange - Put the necessary data in the Database
            var fakeWorld = new World() { Name = "Mars", HasLife = true };
            var savedWorld = await _worldRepo.InsertDocument(fakeWorld);
            await Task.Delay(100);

            //Act - get all worlds from the API
            var response = await _testClient.GetAsync("/api/worlds");
            var responseString = await response.Content.ReadAsStringAsync();
            var expected = JsonSerializer.Deserialize<List<World>>(responseString);

            //Assert - check if the worlds returned are the same as the ones created
            response.StatusCode.Should().Equals(200);
            expected.Should().HaveCount(1);
        }

        [Test]
        public async Task TestThatGetWorldEndpointWorks()
        {
            //Arrange - Put the necessary data in the Database
            var fakeWorld = new World() { Name = "Jupiter", HasLife = true };
            var savedWorld = await _worldRepo.InsertDocument(fakeWorld);
            await Task.Delay(100);

            //Act - get a world from the API
            var response = await _testClient.GetAsync($"/api/worlds/{savedWorld.Id}");
            var responseString = await response.Content.ReadAsStringAsync();
            var expected = JsonSerializer.Deserialize<World>(responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            //Assert - check if the worlds returned are the same as the ones created
            response.StatusCode.Should().Equals(200);
            expected.Should().BeEquivalentTo<World>(savedWorld);
        }

        [Test]
        public async Task TestThatPostWorldsEndpointWorks()
        {
            //Arrange - Put the necessary data in the Database
            var fakeWorld = new World() { Name = "Earth", HasLife = true };
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(fakeWorld), Encoding.UTF8, "application/json");

            //Act - post a world from the API
            HttpResponseMessage response = await _testClient.PostAsync("/api/worlds", httpContent);
            await Task.Delay(100);

            string responseString = await response.Content.ReadAsStringAsync();
            var expected = JsonSerializer.Deserialize<World>(responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            //Assert - check if the worlds returned are the same as the ones created
            response.StatusCode.Should().Be(201);
            expected.Name.Should().Be(fakeWorld.Name);
            expected.HasLife.Should().Be(fakeWorld.HasLife);
        }
    }
}