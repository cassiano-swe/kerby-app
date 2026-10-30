namespace Kerby.Api.Modules.Webhook.Features.ReceiveWebhook;

using System.Text;
using System.Text.Json;
using Kerby.Api.Modules.Webhook.Domain;
using Microsoft.Net.Http.Headers;

public record ReceiveWebhookResponse(Guid EventId, DateTime ReceivedAt);

public static class Endpoint
{
    public static RouteHandlerBuilder MapReceiveWebhookEndpoint(
        this IEndpointRouteBuilder endpoints
    )
    {
        return endpoints
            .MapPost(
                "/in/{publicToken}",
                async Task<IResult> (
                    string publicToken,
                    HttpRequest request,
                    IReceiveWebhookUseCase useCase,
                    CancellationToken cancellationToken
                ) =>
                {
                    if (!TryGetTextEncoding(request.ContentType, out var encoding))
                    {
                        return Results.Problem(
                            statusCode: StatusCodes.Status415UnsupportedMediaType,
                            title: "Only JSON and text content types are supported."
                        );
                    }

                    // TODO: Define and enforce a maximum inbound webhook payload size.
                    using var reader = new StreamReader(
                        request.Body,
                        encoding,
                        detectEncodingFromByteOrderMarks: true,
                        leaveOpen: true
                    );
                    var body = await reader.ReadToEndAsync(cancellationToken);
                    var headers = JsonSerializer.Serialize(
                        request.Headers.ToDictionary(
                            header => header.Key,
                            header => header.Value.ToArray(),
                            StringComparer.OrdinalIgnoreCase
                        )
                    );

                    var webhookEvent = await useCase.ExecuteAsync(
                        publicToken,
                        new WebhookEvent
                        {
                            Headers = headers,
                            Body = body,
                            QueryString = request.QueryString.Value ?? string.Empty,
                        },
                        cancellationToken
                    );

                    return Results.Created(
                        (string?)null,
                        new ReceiveWebhookResponse(
                            webhookEvent.Id,
                            webhookEvent.ReceivedAt
                        )
                    );
                }
            )
            .WithName("ReceiveWebhook")
            .WithTags("Webhooks");
    }

    private static bool TryGetTextEncoding(string? contentType, out Encoding encoding)
    {
        encoding = Encoding.UTF8;

        if (
            string.IsNullOrWhiteSpace(contentType)
            || !MediaTypeHeaderValue.TryParse(contentType, out var parsedContentType)
        )
        {
            return false;
        }

        var mediaType = parsedContentType.MediaType.Value;
        if (string.IsNullOrWhiteSpace(mediaType))
        {
            return false;
        }

        var isJson = mediaType.Equals("application/json", StringComparison.OrdinalIgnoreCase)
            || (
                mediaType.StartsWith("application/", StringComparison.OrdinalIgnoreCase)
                && mediaType.EndsWith("+json", StringComparison.OrdinalIgnoreCase)
            );
        var isText = mediaType.StartsWith("text/", StringComparison.OrdinalIgnoreCase);

        if (!isJson && !isText)
        {
            return false;
        }

        try
        {
            encoding = parsedContentType.Encoding ?? Encoding.UTF8;
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}
