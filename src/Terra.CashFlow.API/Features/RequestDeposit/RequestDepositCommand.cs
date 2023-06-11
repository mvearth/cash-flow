using MediatR;

namespace Terra.CashFlow.API.Features.RequestDeposit
{
    public record class RequestDepositCommand(decimal Amount, Guid AccountId) : IRequest;
}