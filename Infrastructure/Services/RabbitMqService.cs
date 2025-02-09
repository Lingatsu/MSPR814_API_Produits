/*using RabbitMQ.Client;
using System.Text;

namespace ProductApi.Infrastructure.Services;

ConnectionFactory factory = new ConnectionFactory();
// "guest"/"guest" by default, limited to localhost connections
factory.UserName = "guest";
factory.Password = "guest";
factory.VirtualHost = "/";
factory.HostName = "localhost";

IConnection conn = await factory.CreateConnectionAsync();
IChannel ch = await conn.CreateChannelAsync();*/