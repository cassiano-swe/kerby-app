namespace Kerby.Api.Modules.Webhook.Features.ReceiveWebhook;

using Kerby.Api.Abstractions;
using Kerby.Api.Exceptions;
using Kerby.Api.Modules.Webhook.Domain;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public interface IReceiveWebhookUseCase
{
    Task<WebhookEvent> ExecuteAsync(
        string publicToken,
        WebhookEvent request,
        CancellationToken cancellationToken
    );
}

public sealed class ReceiveWebhookUseCase(IContext context) : IReceiveWebhookUseCase
{
    private readonly IContext _context = context;

    public async Task<WebhookEvent> ExecuteAsync(
        string publicToken,
        WebhookEvent request,
        CancellationToken cancellationToken
    )
    {
        var endpoint = await _context.Endpoints
            .AsNoTracking()
            .FirstOrDefaultAsync(
                endpoint => endpoint.PublicToken == publicToken,
                cancellationToken
            );

        if (endpoint is null)
        {
            throw new NotFoundException("Webhook endpoint", publicToken);
        }

        var webhookEvent = new WebhookEvent
        {
            Id = Guid.NewGuid(),
            EndpointId = endpoint.Id,
            Headers = request.Headers,
            Body = request.Body,
            QueryString = request.QueryString,
            ReceivedAt = DateTime.UtcNow,
            DeliveryStatus = DeliveryStatus.Pending,
            AttemptsCount = 0,
            LastStatusCode = null,
        };

        _context.WebhookEvents.Add(webhookEvent);
        _context.OutboxMessages.Add(new OutboxMessage
        {
            Id = Guid.NewGuid(),
            EventId = webhookEvent.Id,
            Payload = JsonSerializer.Serialize(new
            {
                EventId = webhookEvent.Id,
                endpoint.DestinationUrl,
                webhookEvent.Headers,
                webhookEvent.Body,
                webhookEvent.QueryString,
            }),
            CreatedAt = webhookEvent.ReceivedAt,
            ProcessedAt = null,
        });
        await _context.SaveChangesAsync(cancellationToken);

        // TODO: Implement delivery, including the original query string. Before enabling it,
        // define the outbound HTTP timeout and add SSRF protections for DestinationUrl.

        return webhookEvent;
    }
}
