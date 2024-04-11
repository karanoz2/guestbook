using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace test.book.dal.health
{
    public sealed class DbHealthCheck : IHealthCheck
    {
        private readonly IDbHealthCheck _dbHealthCheck;
        public DbHealthCheck(IDbHealthCheck dbHealthCheck) {
            _dbHealthCheck = dbHealthCheck;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var (status, ex) = await _dbHealthCheck.IsHealthyAsync();
            
            if (status) return HealthCheckResult.Healthy();

            var data = ex != null ? new Dictionary<string, object> {
                        { "exeption.message", ex.Message },
                        { "exeption.stack", ex.StackTrace ?? string.Empty }
                    } : [];

            return HealthCheckResult.Unhealthy(data: data);
        }
    }
}
