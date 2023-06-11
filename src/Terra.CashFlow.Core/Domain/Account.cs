using Terra.CashFlow.Core.Domain.Events;

namespace Terra.CashFlow.Core.Domain;

public class Account : AggregateRoot
{
    public Guid Id { get; private set; }

    public Guid CustomerId { get; private set; }

    public decimal Amount { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? ModifiedAt { get; private set; }

    public Account(Guid customerId)
    {
        CustomerId = customerId;
        Amount = 0;
    }

    private IList<Deposit>? _deposits;
    public IEnumerable<Deposit>? Deposits => _deposits;

    public void RequestDeposit(decimal amount)
    {
        var deposit = new Deposit(Id, amount);

        if(_deposits is null)
            _deposits = new List<Deposit>();

        _deposits.Add(deposit);

        AddDomainEvent(new DepositRequestedDomainEvent(Id, amount));
    }

    public void CashIn()
    {
        Amount += _deposits?
                    .Where(d => d.AccountId == Id && d.IsReadyToCash())
                    .Sum(d => d.Amount) ?? decimal.Zero;

        AddDomainEvent(new CashInCompletedDomainEvent(Id, Amount));
    }
}