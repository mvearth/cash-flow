using Microsoft.EntityFrameworkCore;
using Terra.CashFlow.Core.Domain;

namespace Terra.CashFlow.Core.Infrastructure.Context;

public class AccountDbContext : DbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
}
