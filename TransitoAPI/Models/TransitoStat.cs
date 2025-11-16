using Newtonsoft.Json;

namespace TransitoAPI.Models
{
    public class Transitotat
    {

        [JsonProperty("capture_ref")]
        public string CaptureRef { get; set; }

        [JsonProperty("created_at")]
        public string FechaCreado { get; set; }

        [JsonProperty("event_id")]
        public Guid? IdEvento { get; set; }

        [JsonProperty("extra")]
        public object Extra { get; set; }

        [JsonProperty("gate_id")]
        public Guid IdCabina { get; set; }

        [JsonProperty("occurred_at")]
        public string FechaOcurrencia { get; set; }

        [JsonProperty("speed_kmh")]
        public decimal? VelocidadKmh { get; set; }

        [JsonProperty("trace_id")]
        public Guid? IdTrazabilidad { get; set; }

        [JsonProperty("transit_id")]
        public Guid IdTransito { get; set; }

        [JsonProperty("updated_at")]
        public string FechaActualizacion { get; set; }

        [JsonProperty("vehicle_plate")]
        public string PatenteVehiculo { get; set; }

        [JsonProperty("vehicle_type")]
        public string TipoVehiculo { get; set; }
    }
}
