using Microsoft.Extensions.Configuration;

namespace Terra.CashFlow.Core.Infrastructure;

public static class ConfigurationFactory
{
    private static readonly string _dotNetEnvironment = "DOTNET_ENVIRONMENT";
    private static readonly string _aspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

    public static IConfiguration CreateConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder();

        configurationBuilder.AddJsonFile("appsettings.json");

        var aspNetCoreEnvironment = Environment.GetEnvironmentVariable(_aspNetCoreEnvironment);
        var dotnetCoreEnvironment = Environment.GetEnvironmentVariable(_dotNetEnvironment);

        if (!string.IsNullOrWhiteSpace(aspNetCoreEnvironment))
        {
            configurationBuilder.AddJsonFile(
                path: $"appsettings.{aspNetCoreEnvironment}.json",
                optional: true,
                reloadOnChange: true
            );
        }

        if (!string.IsNullOrWhiteSpace(dotnetCoreEnvironment))
        {
            configurationBuilder.AddJsonFile(
                path: $"appsettings.{dotnetCoreEnvironment}.json",
                optional: true,
                reloadOnChange: true
            );
        }

        configurationBuilder.AddEnvironmentVariables();

        return configurationBuilder.Build();
    }
}