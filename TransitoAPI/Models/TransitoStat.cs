
using System.Text.Json.Serialization;

namespace TransitoAPI.Models
{
    public class Transitotat
    {
        [JsonPropertyName("capture_ref")]
        public string CaptureRef { get; set; }

        [JsonPropertyName("created_at")]
        public string FechaCreado { get; set; }

        [JsonPropertyName("event_id")]
        public string IdEvento { get; set; }  

        [JsonPropertyName("extra")]
        public object Extra { get; set; }

        [JsonPropertyName("gate_id")]
        public string IdCabina { get; set; }   

        [JsonPropertyName("occurred_at")]
        public string FechaOcurrencia { get; set; }

        [JsonPropertyName("speed_kmh")]
        public string VelocidadKmh { get; set; } 

        [JsonPropertyName("trace_id")]
        public string IdTrazabilidad { get; set; }

        [JsonPropertyName("transit_id")]
        public string IdTransito { get; set; }

        [JsonPropertyName("updated_at")]
        public string FechaActualizacion { get; set; }

        [JsonPropertyName("vehicle_plate")]
        public string PatenteVehiculo { get; set; }

        [JsonPropertyName("vehicle_type")]
        public string TipoVehiculo { get; set; }
    }
}
