namespace TransitoAPI.Models
{
    using System.Text.Json.Serialization;

    public class ApiCabinasResponse
    {
        [JsonPropertyName("data")]
        public List<CabinaPeaje> Data { get; set; }
    }
}
