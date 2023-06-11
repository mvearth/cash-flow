namespace Terra.CashFlow.API.Features.RequestDeposit
{
    public record RequestDepositRequest(decimal Amount, Guid AccountId);
}