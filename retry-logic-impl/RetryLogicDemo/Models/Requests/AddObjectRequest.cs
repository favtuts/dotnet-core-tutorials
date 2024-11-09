using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class AddObjectRequest
{   
   [JsonProperty("name")]
   public string Name { get; set; }
   [JsonProperty("data")]
   public ObjectData Data { get; set; }
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