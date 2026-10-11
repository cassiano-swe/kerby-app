namespace Kerby.Api.Modules.Client.Features.AddClient;

using Kerby.Api.Modules.Client.Domain;

public record AddClientRequest(string FirstName, string LastName, string Email, string PhoneNumber);
public record AddClientResponse(Guid ClientId, string FirstName, string LastName, string Email, string PhoneNumber);

public static class Endpoint
{
    public static RouteHandlerBuilder MapAddClientEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async Task<IResult> (AddClientRequest request, IAddClientUseCase useCase, CancellationToken cancellationToken) =>
        {
            var client = await useCase.ExecuteAsync(new Client
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
            }, cancellationToken);

            return Results.Created($"/clients/{client.Id}", new AddClientResponse(client.Id, client.FirstName, client.LastName, client.Email, client.PhoneNumber));
        })
        .WithName("AddClient");
    }
}