using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ProductApi.Infrastructure.Services
{
    public class RabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqService(IConfiguration configuration)
        {
            var rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? configuration["RABBITMQ_HOST"];
            var rabbitMqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? configuration["RABBITMQ_USER"];
            var rabbitMqPass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? configuration["RABBITMQ_PASS"];

            var factory = new ConnectionFactory()
            {
                HostName = rabbitMqHost,
                UserName = rabbitMqUser,
                Password = rabbitMqPass
            };

            _connection = CreateConnectionWithRetry(factory);
            _channel = _connection.CreateModel();
        }

        private IConnection CreateConnectionWithRetry(ConnectionFactory factory)
        {
            IConnection connection = null;
            int retries = 0;

            while (connection == null && retries < 5)
            {
                try
                {
                    connection = factory.CreateConnection();
                }
                catch (Exception ex)
                {
                    retries++;
                    Console.WriteLine($"Tentative de connexion échouée ({retries}/5) : {ex.Message}");
                    System.Threading.Thread.Sleep(5000);
                }
            }

            if (connection == null)
            {
                throw new Exception("Impossible de se connecter à RabbitMQ après plusieurs tentatives.");
            }

            return connection;
        }

        // Création d'un exchange durable pour une meilleure distribution des messages
        public void DeclareExchange(string exchangeName, string type = "direct")
        {
            _channel.ExchangeDeclare(exchange: exchangeName, type: type, durable: true, autoDelete: false);
        }

        // Méthode pour envoyer un message JSON avec persistance
        public void SendMessage<T>(T messageObject, string exchange, string routingKey)
        {
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true; // Rend le message durable

            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string messageJson = JsonSerializer.Serialize(messageObject, options);
            var body = Encoding.UTF8.GetBytes(messageJson);

            _channel.BasicPublish(exchange: exchange,
                                routingKey: routingKey,
                                basicProperties: properties,
                                body: body);
        }

        // Consommation avec gestion des erreurs et Dead-Letter Queue
        public void ConsumeMessage(string queueName, string exchangeName, string routingKey, Action<string> onMessageReceived)
        {
            _channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: new Dictionary<string, object>
                                 {
                                     { "x-dead-letter-exchange", "dlx_exchange" } // Dead-Letter Exchange
                                 });

            _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                try
                {
                    onMessageReceived(message);
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors du traitement du message : {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                }
            };
            _channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);
        }

        public void NotifyUser(string message)
        {
            SendMessage(message, "user_exchange", "user_notification");
        }
    }
}
