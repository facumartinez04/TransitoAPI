namespace TransitoAPI.Models
{
    using System.Text.Json.Serialization;

    public class CabinaPeaje
    {
        [JsonPropertyName("gate_id")]
        public Guid Id { get; set; }

        [JsonPropertyName("code")]
        public string Codigo { get; set; }

        [JsonPropertyName("name")]
        public string Nombre { get; set; }

        [JsonPropertyName("state")]
        public string Estado { get; set; }

        [JsonPropertyName("created_at")]
        public string FechaCreacion { get; set; }

        [JsonPropertyName("updated_at")]
        public string FechaActualizacion { get; set; }
    }

}
