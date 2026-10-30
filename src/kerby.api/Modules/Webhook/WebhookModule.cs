namespace Kerby.Api.Modules.Webhook;

using Kerby.Api.Modules.Webhook.Features.AddEndpoint;
using Kerby.Api.Modules.Webhook.Features.ListWebhookEvents;
using Kerby.Api.Modules.Webhook.Features.ReceiveWebhook;

public static class WebhookModule
{
    public static IEndpointRouteBuilder MapWebhookEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var webhooksGroup = endpoints
            .MapGroup("/api/webhooks")
            .WithTags("Webhooks");
        var inboundWebhooksGroup = endpoints
            .MapGroup("/in")
            .WithTags("Webhooks");
        var webhookEventsGroup = endpoints
            .MapGroup("/api/webhook-events")
            .WithTags("Webhook Events");

        webhooksGroup.MapAddEndpointEndpoint();
        inboundWebhooksGroup.MapReceiveWebhookEndpoint();
        webhookEventsGroup.MapListWebhookEventsEndpoint();

        return endpoints;
    }
}
