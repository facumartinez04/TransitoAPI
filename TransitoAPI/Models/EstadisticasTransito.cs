namespace TransitoAPI.Models
{
    public class EstadisticasTransito
    {
        public int TotalTransitos { get; set; }

        public Dictionary<Guid, int> TransitosPorCabina { get; set; }
        public Dictionary<string, int> TransitosPorDia { get; set; }
        public Dictionary<string, int> TransitosPorHora { get; set; }
        public Dictionary<Guid, decimal> VelocidadPromedioPorCabina { get; set; }
        public Dictionary<string, int> TransitosPorTipoVehiculo { get; set; }
    }
}
