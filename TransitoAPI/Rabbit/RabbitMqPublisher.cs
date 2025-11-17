using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace TransitoAPI.Rabbit
{
    public class RabbitMqPublisher : IDisposable
    {
        private readonly RabbitMQ.Client.IConnection _connection;
        private readonly RabbitMQ.Client.IModel _channel;

        public RabbitMqPublisher(IConfiguration config)
        {
            var factory = new RabbitMQ.Client.ConnectionFactory
            {
                Uri = new Uri(config["Rabbit:AmqpUrl"]),
                AutomaticRecoveryEnabled = true,
                RequestedHeartbeat = TimeSpan.FromSeconds(30)
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(
                exchange: "tolling.bus",
                type: ExchangeType.Topic,
                durable: true
            );
        }

        public void PublishLanePassage(object body)
        {
            var json = JsonSerializer.Serialize(body);
            var bytes = Encoding.UTF8.GetBytes(json);

            var props = _channel.CreateBasicProperties();
            props.ContentType = "application/json";
            props.DeliveryMode = 2;

            _channel.BasicPublish(
                exchange: "tolling.bus",
                routingKey: "lane.passage.detected",
                basicProperties: props,
                body: bytes
            );

            Console.WriteLine("🐇 Evento enviado a RabbitMQ.");
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
