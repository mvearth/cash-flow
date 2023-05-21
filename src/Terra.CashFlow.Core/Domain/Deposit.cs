using Terra.CashFlow.Core.Domain.Interfaces;

namespace Terra.CashFlow.Core.Domain;

public class Deposit : IEntity
{
    public Guid Id { get; private set; }

    public Guid AccountId { get; private set; }

    public decimal Amount { get; private set; }

    public DepositStatus Status { get; private set; }

    public Deposit(Guid accountId, decimal amount)
    {
        AccountId = accountId;
        Amount = amount;
        Status = DepositStatus.Requested;
    }

    public void Finish() =>
        Status = DepositStatus.Done;

    public bool IsReadyToCash(Guid accountId) =>
        AccountId == accountId && Status == DepositStatus.Requested;
}

public enum DepositStatus 
{
    Requested,
    Done
}

