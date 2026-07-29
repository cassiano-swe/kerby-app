namespace Kerby.Api.Abstractions;

using Kerby.Api.Modules.Client.Domain;
using Microsoft.EntityFrameworkCore;

public interface IContext
{
    public DbSet<Client> Clients { get; set; }
}
