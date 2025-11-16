using System.Text.Json.Serialization;

namespace TransitoAPI.Models
{
    public class RespuestaCabinas
    {
        [JsonPropertyName("data")]
        public List<CabinaPeaje> Datos { get; set; }
    }
}
