using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class BaseResponse {
    [JsonProperty("error")]
    public string Error {get; set;}
    [JsonProperty("message")]
    public string Message {get; set;}
}