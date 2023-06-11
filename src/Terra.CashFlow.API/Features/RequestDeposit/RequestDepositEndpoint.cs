using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Terra.CashFlow.API.Features.RequestDeposit
{
    public static class RequestDepositEndpoint
    {
        public static async Task<IResult> PostAsync(
            [FromServices] IMediator mediator,
            [FromBody] RequestDepositRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = new RequestDepositCommand(request.Amount, request.AccountId);

            await mediator.Send(command, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            return Results.Ok();
        }
    }
}
