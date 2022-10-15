using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;

namespace Product.API.RabbitMQ;

public class RabbitMQProducer : IRabbitMQProducer
{
    public void SendProductMessage<T>(T message)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();
        channel.QueueDeclare("product", exclusive: false);
        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        channel.BasicPublish(exchange: "", routingKey: "product", body: body);
    }
}
