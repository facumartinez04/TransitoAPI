namespace TransitoAPI.Services.Implementations
{
    using Newtonsoft.Json;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using TransitoAPI.Models;
    using TransitoAPI.Services.Interfaces;



    public class TollService : ITollService
    {
        private readonly HttpClient _http;

        public TollService(HttpClient http)
        {
            _http = http;
        }



        public async Task<CabinaPeaje> CrearCabinaAsync(CabinaCrearDto dto)
        {
            var id = Guid.NewGuid();

            var body = new
            {
                gate_id = id,
                code = dto.Codigo,
                name = dto.Nombre,
                state = dto.Estado
            };

            var res = await _http.PostAsJsonAsync("/api/toll_gates", body);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadFromJsonAsync<CabinaPeaje>();
        }

        public async Task<CabinaPeaje> ObtenerCabinaPorId(Guid id)
        {
            var cabinas = await ObtenerCabinasAsync();
            return cabinas.FirstOrDefault(c => c.Id == id);
        }

        public async Task<CabinaPeaje> ActualizarCabinaAsync(Guid id, CabinaActualizarDto dto)
        {
            var existente = await ObtenerCabinaPorId(id);
            if (existente == null)
                return null;

            var body = new
            {
                code = existente.Codigo,
                created_at = existente.FechaCreacion,
                gate_id = id,
                name = dto.Nombre,
                state = dto.Estado,
                updated_at = DateTime.UtcNow.ToString("o")
            };

            var res = await _http.PutAsJsonAsync($"/api/toll_gates?gate_id={id}", body);

            if (!res.IsSuccessStatusCode)
                return null;

            return await res.Content.ReadFromJsonAsync<CabinaPeaje>();
        }


        public async Task<CabinaPeaje> ParchearCabinaAsync(Guid id, object dto)
        {
            var res = await _http.PatchAsJsonAsync($"/api/toll_gates/{id}", dto);
            if (!res.IsSuccessStatusCode) return null;

            return await res.Content.ReadFromJsonAsync<CabinaPeaje>();
        }

        public async Task<bool> EliminarCabinaAsync(Guid id)
        {
            var res = await _http.DeleteAsync($"/api/toll_gates?rate_id={id}");
            return res.IsSuccessStatusCode;
        }


        public async Task<List<Transito>> ObtenerTransitosAsync()
        {
            var res = await _http.GetFromJsonAsync<RespuestaTransitos>("/api/transits");
            return res?.Datos ?? new();
        }

        public async Task<Transito> CrearTransitoAsync(TransitoCrearDto dto)
        {
            var body = new
            {
                transit_id = Guid.NewGuid(),
                capture_ref = dto.ReferenciaCaptura,
                vehicle_plate = dto.PatenteVehiculo,
                vehicle_type = dto.TipoVehiculo,
                speed_kmh = dto.VelocidadKmh,
                gate_id = dto.IdCabina,
                occurred_at = dto.FechaOcurrencia
            };

            var res = await _http.PostAsJsonAsync("/api/transits", body);
            res.EnsureSuccessStatusCode();

            return await res.Content.ReadFromJsonAsync<Transito>();
        }

        public async Task<Transito> ActualizarTransitoAsync(Guid id, TransitoActualizarDto dto)
        {
            var body = new
            {
                vehicle_plate = dto.PatenteVehiculo,
                vehicle_type = dto.TipoVehiculo,
                speed_kmh = dto.VelocidadKmh
            };

            var res = await _http.PutAsJsonAsync($"/api/transits/{id}", body);
            if (!res.IsSuccessStatusCode) return null;

            return await res.Content.ReadFromJsonAsync<Transito>();
        }

        public async Task<Transito> ParchearTransitoAsync(Guid id, object dto)
        {
            var res = await _http.PatchAsJsonAsync($"/api/transits/{id}", dto);
            if (!res.IsSuccessStatusCode) return null;

            return await res.Content.ReadFromJsonAsync<Transito>();
        }

        public async Task<bool> EliminarTransitoAsync(Guid id)
        {
            var res = await _http.DeleteAsync($"/api/transits/{id}");
            return res.IsSuccessStatusCode;
        }



        public async Task<List<CabinaPeaje>> ObtenerCabinasAsync()
        {
            using var client = new HttpClient();

            var url = "https://fun-bernetta-johannson-systems-v2-ba75677f.koyeb.app/api/toll_gates?order_dir=asc";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                Console.WriteLine("❌ Error API Cabinas: " + err);
                return new List<CabinaPeaje>();
            }

            var json = await response.Content.ReadAsStringAsync();

            var apiResponse = System.Text.Json.JsonSerializer.Deserialize<ApiCabinasResponse>(json);

            return apiResponse?.Data ?? new List<CabinaPeaje>();
        }


        public async Task<EstadisticasTransito> ObtenerEstadisticasAsync()
        {
            using var client = new HttpClient();

            var cabinas = await ObtenerCabinasAsync();
            var todosLosTransitos = new List<Transitotat>();

            foreach (var cabina in cabinas)
            {
                var url =
                    $"https://fun-bernetta-johannson-systems-v2-ba75677f.koyeb.app/api/transits?order_dir=asc&gate_id={cabina.Id}";

                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode) continue;

                var json = await response.Content.ReadAsStringAsync();

                var apiResponse =
                    System.Text.Json.JsonSerializer.Deserialize<ApiResponse<Transitotat>>(json);

                if (apiResponse?.Data != null)
                    todosLosTransitos.AddRange(apiResponse.Data);
            }

            var transitosValidos = todosLosTransitos
                .Where(t =>
                    !string.IsNullOrWhiteSpace(t.FechaOcurrencia) &&
                    DateTime.TryParse(t.FechaOcurrencia, out _))
                .ToList();

            var ultimos100 = transitosValidos
                .OrderByDescending(t => DateTime.Parse(t.FechaOcurrencia))
                .Take(100)
                .ToList();

            decimal GetVelocidad(Transitotat t)
            {
                if (string.IsNullOrWhiteSpace(t.VelocidadKmh)) return 0;
                if (decimal.TryParse(t.VelocidadKmh, out var v)) return v;
                return 0;
            }

            var estadisticas = new EstadisticasTransito
            {
                TotalTransitos = ultimos100.Count,

                TransitosPorCabina = ultimos100
                .GroupBy(t => Guid.Parse(t.IdCabina))
                .ToDictionary(g => g.Key, g => g.Count()),

                TransitosPorDia = ultimos100
                    .GroupBy(t => DateTime.Parse(t.FechaOcurrencia).ToString("yyyy-MM-dd"))
                    .ToDictionary(g => g.Key, g => g.Count()),

                TransitosPorHora = ultimos100
                    .GroupBy(t => DateTime.Parse(t.FechaOcurrencia).ToString("HH"))
                    .ToDictionary(g => g.Key, g => g.Count()),

                VelocidadPromedioPorCabina = ultimos100
                    .Where(t => !string.IsNullOrWhiteSpace(t.VelocidadKmh))
                    .GroupBy(t => Guid.Parse(t.IdCabina))
                    .ToDictionary(
                        g => g.Key,
                        g => g.Average(t => GetVelocidad(t))
                    ),

                TransitosPorTipoVehiculo = ultimos100
                    .GroupBy(t => t.TipoVehiculo)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return estadisticas;
        }

    }
}

