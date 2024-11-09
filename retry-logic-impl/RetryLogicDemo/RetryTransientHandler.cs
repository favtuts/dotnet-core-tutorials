using System.Net;

public class RetryTransientHandler : DelegatingHandler
{
    private readonly int maxRetries;
    private readonly TimeSpan delayBetweenRetries;

    public RetryTransientHandler(HttpMessageHandler innerHandler, int maxRetries, TimeSpan delayBetweenRetries)
        : base(innerHandler)
    {
        this.maxRetries = maxRetries;
        this.delayBetweenRetries = delayBetweenRetries;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        int retries = 0;

        while (true)
        {
            HttpResponseMessage response = null;
            bool shouldRetry = false;

            try
            {
                response = await base.SendAsync(request, cancellationToken);
                shouldRetry = !response.IsSuccessStatusCode && IsTransientStatusCode(response.StatusCode);
            }
            catch (Exception ex)
            {
                shouldRetry = false;
            }

            if (!shouldRetry || retries >= maxRetries)
            {
                return response;
            }

            await Task.Delay(delayBetweenRetries, cancellationToken);
            retries++;
        }
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
            case HttpStatusCode:
                return true;

            default:
                return false;
        }
    }       
}