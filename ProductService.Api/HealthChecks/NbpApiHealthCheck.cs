﻿using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ProductService.Api.HealthChecks
{
    

    public class NbpApiHealthCheck : IHealthCheck
    {
        private const string url = "https://api.nbp.pl/api/exchangerates/tables/a?format=json";

        private readonly IHttpClientFactory httpClientFactory;

        public NbpApiHealthCheck(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using var client = httpClientFactory.CreateClient();

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy();
            }
            else
            {
                return HealthCheckResult.Degraded();
            }

        }
    }
}
