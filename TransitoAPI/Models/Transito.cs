using System.Text.Json.Serialization;

namespace TransitoAPI.Models
{
    public class Transito
    {
        [JsonPropertyName("transit_id")]
        public Guid IdTransito { get; set; }

        [JsonPropertyName("capture_ref")]
        public string ReferenciaCaptura { get; set; }

        [JsonPropertyName("gate_id")]
        public Guid IdCabina { get; set; }


        [JsonIgnore]
        [JsonPropertyName("event_id")]
        public Guid? IdEvento { get; set; }

        [JsonIgnore]
        [JsonPropertyName("trace_id")]
        public Guid? IdTrazabilidad { get; set; }

        [JsonPropertyName("vehicle_plate")]
        public string PatenteVehiculo { get; set; }

        [JsonPropertyName("vehicle_type")]
        public string TipoVehiculo { get; set; }

        [JsonPropertyName("speed_kmh")]
        public decimal? VelocidadKmh { get; set; }

        [JsonPropertyName("created_at")]
        public string FechaCreacion { get; set; } = DateTime.UtcNow.ToString("o");

        [JsonPropertyName("occurred_at")]
        public string FechaOcurrencia { get; set; }

        [JsonIgnore]
        [JsonPropertyName("updated_at")]
        public string? FechaActualizacion { get; set; }
    }
}
