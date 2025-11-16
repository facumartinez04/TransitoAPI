using NetMQ;
using NetMQ.Sockets;
using System.Text.Json;

namespace TransitoAPI.Services.Simulacion
{
    public class SimuladorService
    {
        private bool _corriendo = false;
        private Task _tareaSimulacion;
        private CancellationTokenSource _cts;

        public bool EstaCorriendo => _corriendo;


        public void Iniciar(List<Guid> cabinas, int intervaloMs)
        {
            Iniciar(
                cabinas,
                intervaloMs,
                cantidadVehiculos: -1, 
                probAuto: 0.6,
                probMoto: 0.25,
                probCamion: 0.1,
                probBus: 0.05
            );
        }
        public void Iniciar(
    List<Guid> cabinas,
    int intervaloMs,
    int cantidadVehiculos,
    double probAuto,
    double probMoto,
    double probCamion,
    double probBus)
        {
            if (_corriendo) return;

            _corriendo = true;
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            _tareaSimulacion = Task.Run(() =>
            {
                using var push = new PushSocket("@tcp://*:5555");
                var rnd = new Random();
                int enviados = 0;

                while (!token.IsCancellationRequested &&
                       (cantidadVehiculos == -1 || enviados < cantidadVehiculos))
                {
                    var cabina = cabinas[rnd.Next(cabinas.Count)];

                    var tipo = ResolverTipoVehiculo(probAuto, probMoto, probCamion, probBus, rnd);

                    var mensaje = new
                    {
                        gate_id = cabina,
                        capture_ref = $"SIM_{rnd.Next(1, 9999)}",
                        vehicle_plate = $"SIM{rnd.Next(100, 999)}",
                        vehicle_type = tipo,
                        speed_kmh = rnd.Next(20, 130),
                        occurred_at = DateTime.UtcNow.ToString("o")
                    };

                    string json = JsonSerializer.Serialize(mensaje);
                    push.SendFrame(json);

                    enviados++;
                    Thread.Sleep(intervaloMs);
                }

                _corriendo = false;

            }, token);
        }


        public void Detener()
        {
            if (!_corriendo) return;

            _cts.Cancel();
            _corriendo = false;
        }

        private string ResolverTipoVehiculo(double pA, double pM, double pC, double pB, Random rnd)
        {
            double p = rnd.NextDouble();

            if (p < pA) return "Automovil";
            if (p < pA + pM) return "Moto";
            if (p < pA + pM + pC) return "Camion";
            return "Bus";
        }
    }
}
