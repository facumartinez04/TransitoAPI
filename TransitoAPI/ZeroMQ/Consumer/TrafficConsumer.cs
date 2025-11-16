using Microsoft.AspNetCore.SignalR;
using NetMQ;
using NetMQ.Sockets;
using System.Text.Json;
using TransitoAPI.Models;
using TransitoAPI.Services.Interfaces;

namespace TransitoAPI.ZeroMQ.Consumer
{
    public class TrafficConsumer : BackgroundService
    {
        private readonly IHubContext<TrafficHub> _hub;
        private readonly ITollService _tollService;
        private readonly EventQueueService _queue;


        public TrafficConsumer(IHubContext<TrafficHub> hub, ITollService tollService, EventQueueService queue)
        {
            {
                _hub = hub;
                _tollService = tollService;
                _queue = queue;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("➡️ Iniciando TrafficConsumer...");

            try
            {
                using var pullStd = new PullSocket(">tcp://localhost:5555");
                using var pullCustom = new PullSocket(">tcp://localhost:5560");

                Console.WriteLine("📡 TrafficConsumer escuchando tráfico...");

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        if (pullStd.TryReceiveFrameString(out string msgStd))
                            await ProcesarEvento(msgStd);

                        if (pullCustom.TryReceiveFrameString(out string msgCustom))
                            await ProcesarEvento(msgCustom);
                    }
                    catch (Exception exLoop)
                    {
                        Console.WriteLine($"⚠️ Error interno del loop TrafficConsumer: {exLoop.Message}");
                    }

                    await Task.Delay(5, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ TrafficConsumer NO pudo iniciarse: {ex.Message}");
                Console.WriteLine("➡️ La API seguirá funcionando sin el Consumer.");
            }
        }

        private async Task ProcesarEvento(string jsonRaw)
        {
            try
            {
                await _hub.Clients.All.SendAsync("trafficEvent", JsonDocument.Parse(jsonRaw));

                var data = JsonSerializer.Deserialize<TransitoEventDto>(jsonRaw);
                if (data == null) return;

                var dto = new TransitoCrearDto
                {
                    IdCabina = data.gate_id,
                    PatenteVehiculo = data.vehicle_plate,
                    TipoVehiculo = data.vehicle_type,
                    VelocidadKmh = data.speed_kmh,
                    ReferenciaCaptura = data.capture_ref
                };

                _queue.Cola.Enqueue(dto); 

                Console.WriteLine("📨 Evento encolado");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error procesando evento: {ex.Message}");
            }
        }
    }

        public class TransitoEventDto
    {
        public Guid gate_id { get; set; }
        public string capture_ref { get; set; }
        public string vehicle_plate { get; set; }
        public string vehicle_type { get; set; }
        public int speed_kmh { get; set; }
    }
}
