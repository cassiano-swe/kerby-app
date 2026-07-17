using Kerby.Api.Exceptions;
using Kerby.Api.Modules.Client;
using Kerby.Api.Modules.Client.Features.AddClient;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IAddClientUseCase, AddClientUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/api-reference");
}

app.MapClientEndpoints();

app.UseExceptionHandler();

app.Run();
