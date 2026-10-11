namespace Kerby.Api.Abstractions;

using Kerby.Api.Modules.Client.Domain;
using Kerby.Api.Modules.Webhook.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public interface IContext
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    public EntityEntry Entry(object entity);

    public DbSet<Client> Clients { get; set; }

    public DbSet<Endpoint> Endpoints { get; set; }
}
