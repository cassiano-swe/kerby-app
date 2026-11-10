namespace Kerby.Api.Modules.Webhook.Domain;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }

    public Guid EventId { get; set; }

    public string Payload { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public DateTime? ProcessedAt { get; set; }
}
