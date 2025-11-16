namespace TransitoAPI.Models
{
    public class SimulacionPersonalizadaRequest
    {
        public List<Guid> Cabinas { get; set; }
        public int IntervaloMs { get; set; }
        public int CantidadVehiculos { get; set; } 

        public double ProbAuto { get; set; }
        public double ProbMoto { get; set; }
        public double ProbCamion { get; set; }
        public double ProbBus { get; set; }
    }

  
}
