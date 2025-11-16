using Newtonsoft.Json;

namespace TransitoAPI.Models
{
    public class ApiResponse<T>
    {
        [JsonProperty("data")]
        public List<T> Data { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }
    }
}
