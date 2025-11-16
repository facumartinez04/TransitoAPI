using System.Text.Json.Serialization;

namespace TransitoAPI.Models
{
    public class RespuestaTransitos
    {
        [JsonPropertyName("data")]
        public List<Transito> Datos { get; set; }
    }
}
