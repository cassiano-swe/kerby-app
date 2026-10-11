namespace Kerby.Api.Modules.Webhook.Features.AddEndpoint;

public record AddEndpointRequest(
    Guid ClientId,
    string Name,
    string Provider,
    string DestinationUrl
);

public record AddEndpointResponse(
    Guid EndpointId,
    string PublicToken,
    string PublicUrl,
    string DestinationUrl
);

public static class Endpoint
{
    public static RouteHandlerBuilder MapAddEndpointEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost(
                "/",
                async Task<IResult> (
                    AddEndpointRequest request,
                    IAddEndpointUseCase useCase,
                    HttpContext httpContext,
                    CancellationToken cancellationToken
                ) =>
                {
                    var endpoint = await useCase.ExecuteAsync(
                        new Kerby.Api.Modules.Webhook.Domain.Endpoint
                        {
                            ClientId = request.ClientId,
                            Name = request.Name,
                            Provider = request.Provider,
                            DestinationUrl = request.DestinationUrl,
                        },
                        cancellationToken
                    );
                    var publicBaseUrl =
                        $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
                    return Results.Created(
                        $"/endpoints/{endpoint.Id}",
                        new AddEndpointResponse(
                            endpoint.Id,
                            endpoint.PublicToken,
                            $"{publicBaseUrl}/in/{endpoint.PublicToken}",
                            endpoint.DestinationUrl
                        )
                    );
                }
            )
            .WithName("AddEndpoint");
    }
}
