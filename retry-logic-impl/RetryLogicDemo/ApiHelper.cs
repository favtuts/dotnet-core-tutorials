using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

public class ApiHelper
{
    private readonly HttpClient _httpClient;

    public ApiHelper(string apiServer)
    {
        var httpClientHandler = new HttpClientHandler();
        var retryHandler = new RetrySimpleHandler(httpClientHandler, maxRetries: 3, delayBetweenRetries: TimeSpan.FromSeconds(1));
        _httpClient = new HttpClient(retryHandler)
        {
            BaseAddress = new Uri(apiServer)
        };
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    #region Private Helpers



    private async Task<string> RequestString(HttpRequestMessage requestMessage)
    {
        try {

            var response = await _httpClient.SendAsync(requestMessage);
        await response.EnsureSuccessStatusCodeAsync();        
        return await response.Content.ReadAsStringAsync();        

        } catch (Exception ex) {
            return ex.Message;
        }
        
    }

    private async Task<T> RequestObject<T>(HttpRequestMessage requestMessage)
    {
        var result = await RequestString(requestMessage);
        if (result == null)
        {
            return default(T);
        }
        return JsonConvert.DeserializeObject<T>(result);
    }


    private HttpRequestMessage CreateRequestMessage(HttpMethod httpMethod, string url, object model = null) {
        var requestMessage = new HttpRequestMessage(httpMethod, url);
        if (model != null) {
            JsonSerializerSettings settings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            };
            var modelJsonString = JsonConvert.SerializeObject(model, settings);
            requestMessage.Content = new StringContent(modelJsonString, Encoding.UTF8, "application/json");
        }
        return requestMessage;
    }

    #endregion

    public async Task<T> GetAsync<T>(string url) {
        var requestMessage = CreateRequestMessage(HttpMethod.Get, url);
        return await RequestObject<T>(requestMessage);
    }

    public async Task<T> PostAsync<T>(string url, object model)
    {
        var requestMessage = CreateRequestMessage(HttpMethod.Post, url, model);
        return await RequestObject<T>(requestMessage);
    }        

    public async Task<T> PatchAsync<T>(string url, object model)
    {
        var requestMessage = CreateRequestMessage(new HttpMethod("PATCH"), url, model);
        return await RequestObject<T>(requestMessage);
    }        

    public async Task<T> PutAsync<T>(string url, object model)
    {
        var requestMessage = CreateRequestMessage(HttpMethod.Put, url, model);
        return await RequestObject<T>(requestMessage);
    }       

    public async Task<T> DeleteAsync<T>(string url)
    {
        var requestMessage = CreateRequestMessage(HttpMethod.Delete, url);
        return await RequestObject<T>(requestMessage);
    }
    
}