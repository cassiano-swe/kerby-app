using Kerby.Api.Abstractions;
using Kerby.Api.Exceptions;
using Kerby.Api.Infrastructure.Data;
using Kerby.Api.Modules.Client;
using Kerby.Api.Modules.Client.Features.AddClient;
using Kerby.Api.Modules.Client.Features.DeleteClient;
using Kerby.Api.Modules.Client.Features.GetClient;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<IContext, Context>(options => options.UseInMemoryDatabase("kerbydb"));

builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IAddClientUseCase, AddClientUseCase>();
builder.Services.AddScoped<IGetClientUseCase, GetClientUseCase>();
builder.Services.AddScoped<IDeleteClientUseCase, DeleteClientUseCase>();

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
