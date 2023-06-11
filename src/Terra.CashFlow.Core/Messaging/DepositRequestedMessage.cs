namespace Terra.CashFlow.Core.Messaging;

public record DepositRequestedMessage(Guid AccountId, decimal Amount);