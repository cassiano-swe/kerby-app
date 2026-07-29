namespace Kerby.Api.Modules.Client.Features.DeleteClient;

public static class Endpoint
{
    public static RouteHandlerBuilder MapDeleteClientEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete(
                "/clients/{id}",
                async Task<IResult> (
                    Guid id,
                    IDeleteClientUseCase useCase,
                    CancellationToken cancellationToken
                ) =>
                {
                    await useCase.ExecuteAsync(id, cancellationToken);

                    return Results.Ok();
                }
            )
            .WithName("DeleteClient");
    }
}
