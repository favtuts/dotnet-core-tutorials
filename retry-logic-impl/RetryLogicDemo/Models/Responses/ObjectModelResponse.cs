using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class ObjectModelResponse : BaseResponse {
    [JsonProperty("id")]
    public string Id {get; set;}
    [JsonProperty("name")]
    public string Name {get; set;}
    [JsonProperty("data")]
    public ObjectData Data {get; set;}
}

public class ObjectData {

    [JsonProperty("year")]
    public int Year {get; set;}
    [JsonProperty("price")]
    public double Price {get; set;}
    [JsonProperty("CPU model")]
    public string CPUModel {get; set;}
    [JsonProperty("Hard disk size")]
    public string HardDiskSize {get; set;}
}
/*
{
   "name": "Apple MacBook Pro 16",
   "data": {
      "year": 2019,
      "price": 1849.99,
      "CPU model": "Intel Core i9",
      "Hard disk size": "1 TB"
   }
}
*/