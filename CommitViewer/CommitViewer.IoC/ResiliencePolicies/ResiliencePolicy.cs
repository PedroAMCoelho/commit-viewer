using CommitViewer.Business.CommitViewer;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CommitViewer.IoC.ResiliencePolicies
{
    public class ResiliencePolicy
    {
        public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(15));
        }
    }
}
