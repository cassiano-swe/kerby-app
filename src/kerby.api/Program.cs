using FluentValidation;
using Kerby.Api.Abstractions;
using Kerby.Api.Exceptions;
using Kerby.Api.Infrastructure.Data;
using Kerby.Api.Modules.Client.Domain;
using Kerby.Api.Modules.Client.Features.AddClient;
using Kerby.Api.Modules.Client.Features.DeleteClient;
using Kerby.Api.Modules.Client.Features.GetClient;
using Kerby.Api.Modules.Client.Features.UpdateClient;
using Kerby.Api.Modules.Webhook.Features.AddEndpoint;
using Kerby.Api.Modules.Webhook.Features.ListWebhookEvents;
using Kerby.Api.Modules.Webhook.Features.ReceiveWebhook;
using Kerby.Api.Modules.Client;
using Kerby.Api.Modules.Webhook;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var connectionString = builder.Configuration.GetConnectionString("KerbyDb");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Configure ConnectionStrings:KerbyDb for PostgreSQL.");
}
builder.Services.AddDbContext<IContext, Context>(options => options.UseNpgsql(connectionString));

builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IValidator<AddClientRequest>, AddClientRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateClientRequest>, UpdateClientRequestValidator>();
builder.Services.AddScoped<IValidator<AddEndpointRequest>, AddEndpointRequestValidator>();
builder.Services.AddScoped<
    IValidator<ListWebhookEventsRequest>,
    ListWebhookEventsRequestValidator
>();

builder.Services.AddScoped<IAddClientUseCase, AddClientUseCase>();
builder.Services.AddScoped<IGetClientUseCase, GetClientUseCase>();
builder.Services.AddScoped<IDeleteClientUseCase, DeleteClientUseCase>();
builder.Services.AddScoped<IUpdateClientUseCase, UpdateClientUseCase>();
builder.Services.AddScoped<IAddEndpointUseCase, AddEndpointUseCase>();
builder.Services.AddScoped<IReceiveWebhookUseCase, ReceiveWebhookUseCase>();
builder.Services.AddScoped<IListWebhookEventsUseCase, ListWebhookEventsUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapClientEndpoints();
app.MapWebhookEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/api-reference");
}

app.UseExceptionHandler();

app.Run();
