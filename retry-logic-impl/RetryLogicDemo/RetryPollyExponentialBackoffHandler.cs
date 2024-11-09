using System.Net;
using Polly;
using Polly.Retry;
public class RetryExponentialBackoffHandler : DelegatingHandler
{
    private readonly AsyncRetryPolicy<HttpResponseMessage> retryPolicy;

    public RetryExponentialBackoffHandler(HttpMessageHandler innerHandler, int maxRetries, TimeSpan initialDelay)
        : base(innerHandler)
    {
        this.retryPolicy = Policy
            .HandleResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode 
                                                            && IsTransientStatusCode(response.StatusCode))  
            .WaitAndRetryAsync(
                maxRetries,
                retryAttempt => initialDelay * Math.Pow(2, retryAttempt - 1),
                (response, timespan, retryCount, context) =>
                {
                    if (IsTransientStatusCode(response.Result.StatusCode))
                    {
                        // Log or handle the transient status code
                    }
                }
            );
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