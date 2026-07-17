namespace Kerby.Api.Modules.Client.Features.AddClient;

using Kerby.Api.Modules.Client.Domain;

public interface IAddClientUseCase
{
    Task<Client> ExecuteAsync(Client request, CancellationToken cancellationToken);
}

public sealed class AddClientUseCase : IAddClientUseCase
{
    public async Task<Client> ExecuteAsync(Client request, CancellationToken cancellationToken)
    {
        // Simulate adding the client to a database or performing any necessary logic
        var clientId = Guid.NewGuid();

        // Return the response with the newly created client ID
        return new Client
        {
            Id = clientId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address
        };
    }
}
