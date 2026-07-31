namespace Kerby.Api.Modules.Client.Features.UpdateClient;

using Kerby.Api.Modules.Client.Domain;

public record UpdateClientRequest(string FirstName, string LastName, string Email, string PhoneNumber, string Address);
public record UpdateClientResponse(Guid ClientId, string FirstName, string LastName, string Email, string PhoneNumber, string Address);

public static class Endpoint
{
    public static RouteHandlerBuilder MapUpdateClientEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id}", async Task<IResult> (Guid id, UpdateClientRequest request, IUpdateClientUseCase useCase, CancellationToken cancellationToken) =>
        {
            var client = await useCase.ExecuteAsync(new Client
            {
                Id = id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address
            }, cancellationToken);

            return Results.Ok(new UpdateClientResponse(client.Id, client.FirstName, client.LastName, client.Email, client.PhoneNumber, client.Address));
        })
        .WithName("UpdateClient");
    }
}