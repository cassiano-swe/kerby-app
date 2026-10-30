namespace Kerby.Api.Modules.Webhook.Domain;

public sealed class WebhookEvent
{
    public Guid Id { get; set; }

    public Guid EndpointId { get; set; }

    public string Headers { get; set; } = default!;

    public string Body { get; set; } = default!;

    public string QueryString { get; set; } = default!;

    public DateTime ReceivedAt { get; set; }

    public DeliveryStatus DeliveryStatus { get; set; }

    public int AttemptsCount { get; set; }

    public int? LastStatusCode { get; set; }
}
