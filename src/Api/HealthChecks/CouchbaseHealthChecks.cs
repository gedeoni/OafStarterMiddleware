using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

using Couchbase;

namespace Api.HealthChecks
{
    public class CouchbaseHealthChecks : IHealthCheck
    {
        private readonly string _connectionString;
        private readonly string _userName;
        private readonly string _password;

        public CouchbaseHealthChecks(string connectionString, string userName, string password)
        {
            _connectionString = connectionString;
            _userName = userName;
            _password = password;
        }

        async public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var cluster = await Cluster.ConnectAsync(_connectionString, _userName, _password);
                if(cluster != null)
                {
                    return HealthCheckResult.Healthy("Good");
                }

                return HealthCheckResult.Degraded("UnHealth");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Degraded($"UnHealth: {ex.Message}");
            }
        }
    }
}
