using MediatR;

namespace Terra.CashFlow.Core.Domain.Events;

public record CashInCompletedDomainEvent(Guid AccountId, decimal NewAmount) : INotification;
