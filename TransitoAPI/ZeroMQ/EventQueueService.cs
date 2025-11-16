using System.Collections.Concurrent;
using TransitoAPI.Models;

namespace TransitoAPI.ZeroMQ
{
    public class EventQueueService
    {
        public ConcurrentQueue<TransitoCrearDto> Cola = new();
    }
}
