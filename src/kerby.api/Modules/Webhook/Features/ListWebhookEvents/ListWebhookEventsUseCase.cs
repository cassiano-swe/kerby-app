namespace Kerby.Api.Modules.Webhook.Features.ListWebhookEvents;

using Kerby.Api.Abstractions;
using Kerby.Api.Modules.Webhook.Domain;
using Microsoft.EntityFrameworkCore;

public sealed record ListWebhookEventsFilter(
    Guid? ClientId,
    Guid? EndpointId,
    DeliveryStatus? Status,
    DateTime? StartDate,
    DateTime? EndDate,
    int Page,
    int PageSize
);

public sealed record WebhookEventListItem(
    Guid EventId,
    string EndpointName,
    string Provider,
    DateTime ReceivedAt,
    DeliveryStatus DeliveryStatus,
    int AttemptsCount,
    int? LastStatusCode
);

public sealed record WebhookEventList(
    IReadOnlyList<WebhookEventListItem> Items,
    int Page,
    int PageSize,
    int TotalCount
);

public interface IListWebhookEventsUseCase
{
    Task<WebhookEventList> ExecuteAsync(
        ListWebhookEventsFilter filter,
        CancellationToken cancellationToken
    );
}

public sealed class ListWebhookEventsUseCase(IContext context) : IListWebhookEventsUseCase
{
    private readonly IContext _context = context;

    public async Task<WebhookEventList> ExecuteAsync(
        ListWebhookEventsFilter filter,
        CancellationToken cancellationToken
    )
    {
        var query =
            from webhookEvent in _context.WebhookEvents.AsNoTracking()
            join endpoint in _context.Endpoints.AsNoTracking()
                on webhookEvent.EndpointId equals endpoint.Id
            select new { WebhookEvent = webhookEvent, Endpoint = endpoint };

        if (filter.ClientId.HasValue)
        {
            query = query.Where(item => item.Endpoint.ClientId == filter.ClientId.Value);
        }

        if (filter.EndpointId.HasValue)
        {
            query = query.Where(item => item.WebhookEvent.EndpointId == filter.EndpointId.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(item => item.WebhookEvent.DeliveryStatus == filter.Status.Value);
        }

        if (filter.StartDate.HasValue)
        {
            query = query.Where(item => item.WebhookEvent.ReceivedAt >= filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            query = query.Where(item => item.WebhookEvent.ReceivedAt < filter.EndDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(item => item.WebhookEvent.ReceivedAt)
            .ThenByDescending(item => item.WebhookEvent.Id)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(item => new WebhookEventListItem(
                item.WebhookEvent.Id,
                item.Endpoint.Name,
                item.Endpoint.Provider,
                item.WebhookEvent.ReceivedAt,
                item.WebhookEvent.DeliveryStatus,
                item.WebhookEvent.AttemptsCount,
                item.WebhookEvent.LastStatusCode
            ))
            .ToListAsync(cancellationToken);

        return new WebhookEventList(items, filter.Page, filter.PageSize, totalCount);
    }
}
