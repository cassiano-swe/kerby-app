namespace Kerby.Api.Infrastructure.Data;

using Kerby.Api.Abstractions;
using Kerby.Api.Modules.Client.Domain;
using Kerby.Api.Modules.Webhook.Domain;
using Microsoft.EntityFrameworkCore;

public class Context(DbContextOptions<Context> options) : DbContext(options), IContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>().HasKey(client => client.Id);
        modelBuilder.Entity<Endpoint>().HasKey(endpoint => endpoint.Id);
        modelBuilder.Entity<WebhookEvent>().HasKey(webhookEvent => webhookEvent.Id);

        modelBuilder.Entity<Endpoint>()
            .HasOne<Client>()
            .WithMany()
            .HasForeignKey(endpoint => endpoint.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WebhookEvent>()
            .HasOne<Endpoint>()
            .WithMany()
            .HasForeignKey(webhookEvent => webhookEvent.EndpointId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Endpoint>().Property(endpoint => endpoint.CreatedAt)
            .HasColumnType("timestamp with time zone");
        modelBuilder.Entity<WebhookEvent>().Property(webhookEvent => webhookEvent.ReceivedAt)
            .HasColumnType("timestamp with time zone");
    }

    public DbSet<Client> Clients { get; set; }

    public DbSet<Endpoint> Endpoints { get; set; }

    public DbSet<WebhookEvent> WebhookEvents { get; set; }
}
