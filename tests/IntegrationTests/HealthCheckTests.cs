using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace IntegrationTests
{
    using static Testing;

    public class HealthCheckTests
    {
        [Test]
        public async Task TestThatHealthEndpointWorks()
        {
            var response = await client.GetAsync("/health");
            var expected  = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(200);
            expected .Should().Contain("\"status\":\"Healthy");
        }
    }
}
