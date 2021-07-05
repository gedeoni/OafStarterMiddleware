using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Api.HealthChecks
{
    public static class CouchbaseHealthCheckBuilderExtensions
    {
        private const string NAME = "Couchbase";

        public static IHealthChecksBuilder AddCouchbaseHealthCheck(
            this IHealthChecksBuilder builder,
            string couchbaseConnectionString,
            string username,
            string password,
            string name = default,
            HealthStatus? failureStatus = default,
            IEnumerable<string> tags = default,
            TimeSpan? timeout = default
        )
        {
            return builder.Add(new HealthCheckRegistration(
                name ?? NAME,
                sp => new CouchbaseHealthChecks(couchbaseConnectionString, username, password),
                failureStatus,
                tags,
                timeout));
        }
    }
}
