using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class UpdateObjectResponse : ObjectModelResponse
{
   [JsonProperty("updatedAt")]
   public string UpdatedAt { get; set; }
}
/*
{
   "id": "7",
   "name": "Apple MacBook Pro 16",
   "data": {
      "year": 2019,
      "price": 1849.99,
      "CPU model": "Intel Core i9",
      "Hard disk size": "1 TB"
   },
   "createdAt": "2022-11-21T20:06:23.986Z"
}
*/