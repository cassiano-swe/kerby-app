namespace Kerby.Api.Modules.Webhook.Domain;

public sealed class Endpoint
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public string Name { get; set; } = default!;

    public string Provider { get; set; } = default!;

    public string DestinationUrl { get; set; } = default!;

    public string PublicToken { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

}