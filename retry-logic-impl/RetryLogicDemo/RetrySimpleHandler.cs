public class RetrySimpleHandler : DelegatingHandler
{
    private readonly int maxRetries;
    private readonly TimeSpan delayBetweenRetries;

    public RetrySimpleHandler(HttpMessageHandler innerHandler, int maxRetries, TimeSpan delayBetweenRetries)
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
            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode || retries >= maxRetries)
                {
                    return response;
                }
            }
            catch (Exception)
            {
                if (retries >= maxRetries)
                {
                    throw;
                }
            }

            await Task.Delay(delayBetweenRetries, cancellationToken);
            retries++;
        }
    }
}