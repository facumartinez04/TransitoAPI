using Newtonsoft.Json;

namespace TransitoAPI.Models
{
    public class Transito
    {
        [JsonProperty("transit_id")]
        public Guid IdTransito { get; set; }

        [JsonProperty("capture_ref")]
        public string ReferenciaCaptura { get; set; }

        [JsonProperty("gate_id")]
        public Guid IdCabina { get; set; }


        [JsonIgnore]
        [JsonProperty("event_id")]
        public Guid? IdEvento { get; set; }

        [JsonIgnore]
        [JsonProperty("trace_id")]
        public Guid? IdTrazabilidad { get; set; }

        [JsonProperty("vehicle_plate")]
        public string PatenteVehiculo { get; set; }

        [JsonProperty("vehicle_type")]
        public string TipoVehiculo { get; set; }

        [JsonProperty("speed_kmh")]
        public decimal? VelocidadKmh { get; set; }

        [JsonProperty("created_at")]
        public string FechaCreacion { get; set; } = DateTime.UtcNow.ToString("o");

        [JsonProperty("occurred_at")]
        public string FechaOcurrencia { get; set; }

        [JsonIgnore]
        [JsonProperty("updated_at")]
        public string? FechaActualizacion { get; set; }
    }
}
