using MediatR;

namespace Terra.CashFlow.Core.Domain.Events;

public record DepositRequestedDomainEvent(Guid AccountId, decimal Ammount) : INotification;
