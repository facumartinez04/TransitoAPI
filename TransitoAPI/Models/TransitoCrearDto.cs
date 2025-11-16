namespace TransitoAPI.Models
{
    public class TransitoCrearDto
    {
        public Guid Id { get; set; }
        public string? ReferenciaCaptura { get; set; }
        public string PatenteVehiculo { get; set; }
        public string TipoVehiculo { get; set; }
        public decimal? VelocidadKmh { get; set; }
        public Guid IdCabina { get; set; }

        public string FechaOcurrencia = DateTime.UtcNow.ToString("o");

    }
}
