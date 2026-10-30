namespace Kerby.Api.Modules.Webhook.Features.ListWebhookEvents;

using FluentValidation;
using Kerby.Api.Modules.Webhook.Domain;
using Microsoft.AspNetCore.Mvc;

public sealed class ListWebhookEventsRequest
{
    public Guid? ClientId { get; init; }

    public Guid? EndpointId { get; init; }

    public string? Status { get; init; }

    public DateTime? StartDate { get; init; }

    public DateTime? EndDate { get; init; }

    public int? Page { get; init; }

    public int? PageSize { get; init; }
}

public sealed record ListWebhookEventResponse(
    Guid EventId,
    string EndpointName,
    string Provider,
    DateTime ReceivedAt,
    string DeliveryStatus,
    int AttemptsCount,
    int? LastStatusCode
);

public sealed record ListWebhookEventsResponse(
    IReadOnlyList<ListWebhookEventResponse> Items,
    int Page,
    int PageSize,
    int TotalCount
);

public static class Endpoint
{
    public static RouteHandlerBuilder MapListWebhookEventsEndpoint(
        this IEndpointRouteBuilder endpoints
    )
    {
        return endpoints
            .MapGet(
                "/",
                async Task<IResult> (
                    [AsParameters] ListWebhookEventsRequest request,
                    IListWebhookEventsUseCase useCase,
                    IValidator<ListWebhookEventsRequest> validator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var validationResult = await validator.ValidateAsync(
                        request,
                        cancellationToken
                    );
                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }

                    DeliveryStatus? status = null;
                    if (!string.IsNullOrWhiteSpace(request.Status))
                    {
                        status = Enum.Parse<DeliveryStatus>(request.Status, ignoreCase: true);
                    }

                    var page = request.Page ?? 1;
                    var pageSize = request.PageSize ?? 50;

                    var result = await useCase.ExecuteAsync(
                        new ListWebhookEventsFilter(
                            request.ClientId,
                            request.EndpointId,
                            status,
                            request.StartDate,
                            request.EndDate,
                            page,
                            pageSize
                        ),
                        cancellationToken
                    );

                    return Results.Ok(
                        new ListWebhookEventsResponse(
                            result.Items
                                .Select(item => new ListWebhookEventResponse(
                                    item.EventId,
                                    item.EndpointName,
                                    item.Provider,
                                    item.ReceivedAt,
                                    item.DeliveryStatus.ToString(),
                                    item.AttemptsCount,
                                    item.LastStatusCode
                                ))
                                .ToList(),
                            result.Page,
                            result.PageSize,
                            result.TotalCount
                        )
                    );
                }
            )
            .WithName("ListWebhookEvents");
    }
}
