namespace Product.API.RabbitMQ;

public interface IRabbitMQProducer
{
    public void SendProductMessage<T>(T message);
}