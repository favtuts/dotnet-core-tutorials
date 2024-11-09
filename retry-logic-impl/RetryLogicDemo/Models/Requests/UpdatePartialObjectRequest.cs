using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class UpdatePartialObjectRequest
{
    [JsonProperty("name")]
    public string Name { get; set; }
}