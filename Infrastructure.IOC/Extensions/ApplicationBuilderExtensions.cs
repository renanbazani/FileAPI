using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.IOC.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication ConfigureHealthCheck(this WebApplication app)
        {
            app.MapHealthChecks("/health/readiness", new HealthCheckOptions()
            {
                Predicate = (check) => check.Tags.Contains("ready"),
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                }
            });

            app.MapHealthChecks("/health/liveness", new HealthCheckOptions()
            {
                Predicate = (check) => check.Tags.Contains("live"),
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                }
            });

            return app;
        }
    }
}
