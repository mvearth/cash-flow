using Microsoft.AspNetCore.Mvc;

namespace Terra.CashFlow.API.AddTransferFeature.Controllers;

[ApiController]
[Route("[controller]")]
public class TransferController : ControllerBase
{
    private readonly ILogger<TransferController> _logger;

    public TransferController(ILogger<TransferController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "PostTransfer")]
    public IEnumerable<WeatherForecast> Post()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
