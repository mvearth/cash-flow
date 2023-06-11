using MediatR;
using Microsoft.EntityFrameworkCore;
using Terra.CashFlow.Core.Infrastructure.Context;

namespace Terra.CashFlow.API.Features.RequestDeposit;

public class RequestDepositCommandHandler : IRequestHandler<RequestDepositCommand>
{
    private readonly AccountDbContext _context;
    private readonly IMediator _mediator;

    public RequestDepositCommandHandler(AccountDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task Handle(RequestDepositCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _context.Database.BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == request.AccountId, cancellationToken: cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false)
                ?? throw new Exception("Account not found.");

            account.RequestDeposit(request.Amount);

            await _context.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            await _context.Database.CommitTransactionAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            var dispatchingTasks = account.DomainEvents.Select(domainEvent => _mediator.Publish(domainEvent, cancellationToken));

            await Task.WhenAll(dispatchingTasks);
        }
        catch (Exception ex)
        {
            await _context.Database.RollbackTransactionAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            throw;
        }
    }
}
