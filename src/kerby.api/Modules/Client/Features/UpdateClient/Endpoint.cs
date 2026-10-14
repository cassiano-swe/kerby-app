namespace Kerby.Api.Modules.Client.Features.UpdateClient;

using FluentValidation;
using Kerby.Api.Modules.Client.Domain;

public record UpdateClientRequest(string FirstName, string LastName, string Email, string PhoneNumber);
public record UpdateClientResponse(Guid ClientId, string FirstName, string LastName, string Email, string PhoneNumber);

public static class Endpoint
{
    public static RouteHandlerBuilder MapUpdateClientEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id}", async Task<IResult> (
            Guid id,
            UpdateClientRequest request,
            IUpdateClientUseCase useCase,
            IValidator<UpdateClientRequest> validator,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var client = await useCase.ExecuteAsync(new Client
            {
                Id = id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
            }, cancellationToken);

            return Results.Ok(new UpdateClientResponse(client.Id, client.FirstName, client.LastName, client.Email, client.PhoneNumber));
        })
        .WithName("UpdateClient");
    }
}