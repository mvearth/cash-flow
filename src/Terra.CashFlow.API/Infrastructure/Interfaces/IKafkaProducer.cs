namespace Terra.CashFlow.API.Infrastructure.Interfaces;

public interface IKafkaProducer
{
    Task ProduceMessageAsync<TMessage>(string topic, TMessage message, CancellationToken cancellationToken) where TMessage : class;
}