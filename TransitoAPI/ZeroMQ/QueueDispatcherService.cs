using TransitoAPI.Services.Interfaces;

namespace TransitoAPI.ZeroMQ
{
    public class QueueDispatcherService : BackgroundService
    {
        private readonly EventQueueService _queue;
        private readonly ITollService _tollService;
        private readonly int _intervaloMs = 100; 

        public QueueDispatcherService(EventQueueService queue, ITollService tollService)
        {
            _queue = queue;
            _tollService = tollService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("🚚 Dispatcher iniciado...");

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_queue.Cola.TryDequeue(out var dto))
                {
                    try
                    {
                        await _tollService.CrearTransitoAsync(dto);
                        Console.WriteLine("💾 Tránsito enviado correctamente");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("❌ Error enviando tránsito: " + ex.Message);
                        _queue.Cola.Enqueue(dto);
                    }
                }

                await Task.Delay(_intervaloMs, stoppingToken);
            }
        }
    }
}
