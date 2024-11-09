using System.Net;
using Polly;
using Polly.Retry;
public class RetryCircuitBreakerHandlerHandler : DelegatingHandler
{
    private readonly AsyncRetryPolicy<HttpResponseMessage> retryPolicy;

    public RetryCircuitBreakerHandlerHandler(HttpMessageHandler innerHandler, int maxRetries, TimeSpan initialDelay, int circuitBreakerThreshold, TimeSpan circuitBreakerDuration)
        : base(innerHandler)
    {
        var retryPolicy = Policy
        .HandleResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode
                                                            && IsTransientStatusCode(response.StatusCode))
        .Or<Exception>()
        .WaitAndRetryAsync(
            maxRetries,
            retryAttempt => initialDelay * Math.Pow(2, retryAttempt - 1)
        );

        this.retryPolicy = Policy
            .HandleResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode
                                                                && IsTransientStatusCode(response.StatusCode))
            .AdvancedCircuitBreakerAsync(
                failureThreshold: circuitBreakerThreshold,
                samplingDuration: circuitBreakerDuration,
                onBreak: (ex, breakDelay) =>
                {
                    // Log or handle the circuit breaker state change (e.g., open)
                },
                onReset: () =>
                {
                    // Log or handle the circuit breaker state change (e.g., closed)
                }
            )
            .WrapAsync(retryPolicy);
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await retryPolicy.ExecuteAsync(async () => await base.SendAsync(request, cancellationToken));
    }

    private bool IsTransientStatusCode(HttpStatusCode statusCode)
    {
        switch (statusCode)
        {
            // List of known transient status codes
            case HttpStatusCode.RequestTimeout:
            case HttpStatusCode.TooManyRequests:
            case HttpStatusCode.ServiceUnavailable:
            case HttpStatusCode.GatewayTimeout:
            case HttpStatusCode.BadGateway:
            case HttpStatusCode.InsufficientStorage:
                return true;

            default:
                return false;
        }
    }
}