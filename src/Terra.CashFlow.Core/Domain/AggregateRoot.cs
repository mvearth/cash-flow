using MediatR;
using Terra.CashFlow.Core.Domain.Interfaces;

namespace Terra.CashFlow.Core.Domain;

public abstract class AggregateRoot : IEntity
{
    private readonly List<INotification> _domainEvents = new();

    public Guid Id { get; protected set; } = default!;

    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification domainEvent) => _domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(INotification domainEvent) => _domainEvents.Remove(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}