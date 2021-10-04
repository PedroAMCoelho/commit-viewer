using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace CommitViewer.IoC.ResiliencePolicies
{
    public class ResiliencePolicy
    {
        public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(1, TimeSpan.FromSeconds(15));
        }
    }
}
