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

        // Méthode pour envoyer un message JSON
        public void SendMessage<T>(T messageObject, string queueName)
        {
            _channel.QueueDeclare(queue: queueName,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            // Sérialisation JSON avec options pour ne pas échapper les caractères Unicode
            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Évite l'échappement des caractères spéciaux
            };
            string messageJson = JsonSerializer.Serialize(messageObject, options);
            var body = Encoding.UTF8.GetBytes(messageJson);

            _channel.BasicPublish(exchange: "",
                                routingKey: queueName,
                                basicProperties: null,
                                body: body);
        }

        public void ConsumeMessage(string queueName, Action<string> onMessageReceived)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                onMessageReceived(message);
            };
            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void NotifyUser(string message)
        {
            SendMessage(message, "user_queue");
        }
    }
}
