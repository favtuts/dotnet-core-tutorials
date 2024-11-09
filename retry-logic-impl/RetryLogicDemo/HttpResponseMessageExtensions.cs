using System.Net;

/// <summary>
/// https://stackoverflow.com/questions/21097730/usage-of-ensuresuccessstatuscode-and-handling-of-httprequestexception-it-throws
/// </summary>
public static class HttpResponseMessageExtensions
{
    public static async Task EnsureSuccessStatusCodeAsync(this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var content = await response.Content.ReadAsStringAsync();

        if (response.Content != null)
            response.Content.Dispose();

        throw new SimpleHttpResponseException(response.StatusCode, content);
    }

    public static void EnsureSuccessStatusCode(this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        if (response.Content != null)
            response.Content.Dispose();

        throw new SimpleHttpResponseException(response.StatusCode, content);
    }
}

public class SimpleHttpResponseException : Exception
{
    public HttpStatusCode StatusCode { get; private set; }

    public SimpleHttpResponseException(HttpStatusCode statusCode, string content) : base(content)
    {
        StatusCode = statusCode;
    }
}