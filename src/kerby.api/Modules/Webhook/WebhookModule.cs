namespace Kerby.Api.Modules.Webhook;

using Kerby.Api.Modules.Webhook.Features.AddEndpoint;

public static class WebhookModule
{
    public static IEndpointRouteBuilder MapWebhookEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/webhooks").WithTags("Webhooks");

        group.MapAddEndpointEndpoint();

        return group;
    }
}
