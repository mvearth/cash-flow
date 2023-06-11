using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Terra.CashFlow.Core.Infrastructure.Context;

public class AccountDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AccountDbContext>
{
    public AccountDbContext CreateDbContext(string[] args)
    {
        var configuration = ConfigurationFactory.CreateConfiguration();

        var connectionString = configuration.GetConnectionString(nameof(AccountDbContext));

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Connection string not found.");
        }

        var builder = new DbContextOptionsBuilder<AccountDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new AccountDbContext(builder.Options);
    }
}