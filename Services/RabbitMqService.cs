/*using RabbitMQ.Client;
using System;
using System.Text;
using Newtonsoft.Json;

public class RabbitMqService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqService()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost", // Remplace par l'IP de RabbitMQ si besoin
            Port = 5672, // Port par défaut
            UserName = "guest", // Identifiants par défaut
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Déclare une file d’attente pour s'assurer qu'elle existe
        _channel.QueueDeclare(queue: "product_queue",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public void PublishMessage(object message)
    {
        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(exchange: "",
                             routingKey: "product_queue",
                             basicProperties: null,
                             body: body);

        Console.WriteLine($"[x] Message envoyé : {json}");
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
*/