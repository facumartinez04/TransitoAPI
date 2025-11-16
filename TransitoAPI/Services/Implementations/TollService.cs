namespace TransitoAPI.Services.Implementations
{
    using System.Net.Http.Json;
    using TransitoAPI.Models;
    using TransitoAPI.Services.Interfaces;

    public class TollService : ITollService
    {
        private readonly HttpClient _http;

        public TollService(HttpClient http)
        {
            _http = http;
        }

   
        public async Task<List<CabinaPeaje>> ObtenerCabinasAsync()
        {
            var res = await _http.GetFromJsonAsync<RespuestaCabinas>("/api/toll_gates");
            return res?.Datos ?? new();
        }

        public async Task<CabinaPeaje> CrearCabinaAsync(CabinaCrearDto dto)
        {
            var id =  Guid.NewGuid();

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


        public async Task<EstadisticasTransito> ObtenerEstadisticasAsync()
        {
            var transitos = await ObtenerTransitosAsync();

            var estadisticas = new EstadisticasTransito
            {
                TotalTransitos = transitos.Count,
                TransitosPorCabina = transitos
                    .GroupBy(t => t.IdCabina)
                    .ToDictionary(g => g.Key, g => g.Count()),

                TransitosPorDia = transitos
                    .GroupBy(t => DateTime.Parse(t.FechaOcurrencia).ToString("yyyy-MM-dd"))
                    .ToDictionary(g => g.Key, g => g.Count()),

                TransitosPorHora = transitos
                    .GroupBy(t => DateTime.Parse(t.FechaOcurrencia).ToString("HH"))
                    .ToDictionary(g => g.Key, g => g.Count()),

                VelocidadPromedioPorCabina = transitos
                    .Where(t => t.VelocidadKmh.HasValue)
                    .GroupBy(t => t.IdCabina)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Average(t => t.VelocidadKmh!.Value)
                    ),

                TransitosPorTipoVehiculo = transitos
                    .GroupBy(t => t.TipoVehiculo)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return estadisticas;
        }
    }

}
