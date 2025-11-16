namespace TransitoAPI.Models
{

    public class SimulacionRequest
    {
        public List<Guid> Cabinas { get; set; }
        public int IntervaloMs { get; set; }

    }
}
