namespace Kerby.Api.Infrastructure.Data;

using Kerby.Api.Abstractions;
using Kerby.Api.Modules.Client.Domain;
using Microsoft.EntityFrameworkCore;

public class Context(DbContextOptions<Context> options) : DbContext(options), IContext
{
    public DbSet<Client> Clients { get; set; }
}