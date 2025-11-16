using TransitoAPI.Models;

namespace TransitoAPI.Services.Interfaces
{
    public interface ITollService
    {
        Task<List<CabinaPeaje>> ObtenerCabinasAsync();
        Task<CabinaPeaje> CrearCabinaAsync(CabinaCrearDto dto);
        Task<CabinaPeaje> ActualizarCabinaAsync(Guid id, CabinaActualizarDto dto);
        Task<CabinaPeaje> ParchearCabinaAsync(Guid id, object dto);
        Task<bool> EliminarCabinaAsync(Guid id);

        Task<List<Transito>> ObtenerTransitosAsync();
        Task<Transito> CrearTransitoAsync(TransitoCrearDto dto);
        Task<Transito> ActualizarTransitoAsync(Guid id, TransitoActualizarDto dto);
        Task<Transito> ParchearTransitoAsync(Guid id, object dto);
        Task<bool> EliminarTransitoAsync(Guid id);

        Task<EstadisticasTransito> ObtenerEstadisticasAsync();

    }
}
