using Confluent.Kafka;
using MediatR.NotificationPublishers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Terra.CashFlow.API.Features.RequestDeposit;
using Terra.CashFlow.API.Infrastructure;
using Terra.CashFlow.API.Infrastructure.Interfaces;
using Terra.CashFlow.Core.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AccountDbContext>((provider, optionsBuilder) =>
{
    var connectionString = builder.Configuration.GetConnectionString(nameof(AccountDbContext));

    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

}, ServiceLifetime.Scoped);

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblyContaining<Program>();
    configuration.NotificationPublisherType = typeof(TaskWhenAllPublisher);
});

builder.Services.TryAdd(new ServiceDescriptor(typeof(ProducerConfig), new ProducerConfig()
{
    BootstrapServers = builder.Configuration.GetValue<string>("Kafka:Servers"),
}));

builder.Services.TryAddScoped<IKafkaProducer, KafkaProducer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/deposits", RequestDepositEndpoint.PostAsync).WithOpenApi();

app.Run();
