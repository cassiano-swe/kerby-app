namespace Kerby.Api.Infrastructure.Data;

using Kerby.Api.Abstractions;
using Kerby.Api.Modules.Client.Domain;
using Kerby.Api.Modules.Webhook.Domain;
using Microsoft.EntityFrameworkCore;

public class Context(DbContextOptions<Context> options) : DbContext(options), IContext
{
    public DbSet<Client> Clients { get; set; }

    public DbSet<Endpoint> Endpoints { get; set; }
}