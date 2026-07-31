namespace Kerby.Api.Modules.Client.Features.GetClient;

public record GetClientResponse(Guid ClientId, string FirstName, string LastName, string Email, string PhoneNumber, string Address);

public static class Endpoint
{
    public static RouteHandlerBuilder MapGetClientEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id}", async Task<IResult> (Guid id, IGetClientUseCase useCase, CancellationToken cancellationToken) =>
        {
            var client = await useCase.ExecuteAsync(id, cancellationToken);

            if (client == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(new GetClientResponse(client.Id, client.FirstName, client.LastName, client.Email, client.PhoneNumber, client.Address));
        })
        .WithName("GetClient");
    }
}