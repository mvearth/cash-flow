using MediatR;
using Terra.CashFlow.API.Infrastructure.Interfaces;
using Terra.CashFlow.Core.Domain.Events;
using Terra.CashFlow.Core.Messaging;

namespace Terra.CashFlow.API.Features.RequestDeposit;

public class DepositRequestedDomainEventHandler : INotificationHandler<DepositRequestedDomainEvent>
{
    private readonly IKafkaProducer _kafkaProducer;

    private const string _topic = "Terra.CashFlow.Queueing.DepositRequested";

    public DepositRequestedDomainEventHandler(IKafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }

    public async Task Handle(DepositRequestedDomainEvent notification, CancellationToken cancellationToken)
    {
        var message = new DepositRequestedMessage(notification.AccountId, notification.Ammount);

        await _kafkaProducer.ProduceMessageAsync(_topic, message, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}
