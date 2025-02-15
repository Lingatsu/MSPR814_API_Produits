using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System;

namespace ProductApi.Infrastructure.Services
{
    public class RabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqService(IConfiguration configuration)
        {
            // Récupération des variables d'environnement ou des paramètres de configuration
            var rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? configuration["RABBITMQ_HOST"];
            var rabbitMqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? configuration["RABBITMQ_USER"];
            var rabbitMqPass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? configuration["RABBITMQ_PASS"];

            // Création de la factory RabbitMQ
            var factory = new ConnectionFactory()
            {
                HostName = rabbitMqHost, // Utilisation du nom de service Docker
                UserName = rabbitMqUser,
                Password = rabbitMqPass
            };

            // Tentative de connexion avec un retry en cas d'échec
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
                    System.Threading.Thread.Sleep(5000); // Attendre 5 secondes avant de réessayer
                }
            }

            if (connection == null)
            {
                throw new Exception("Impossible de se connecter à RabbitMQ après plusieurs tentatives.");
            }

            return connection;
        }

        // Méthode pour envoyer un message à une file RabbitMQ
        public void SendMessage(string message, string queueName)
        {
            _channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: null,
                                 body: body);
        }

        // Méthode pour consommer des messages d'une file RabbitMQ
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

        // Méthode spécifique pour notifier un utilisateur
        public void NotifyUser(string message)
        {
            SendMessage(message, "user_queue");
        }
    }
}
