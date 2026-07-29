namespace Kerby.Api.Modules.Client.Features.AddClient;

using Kerby.Api.Abstractions;
using Kerby.Api.Modules.Client.Domain;

public interface IAddClientUseCase
{
    Task<Client> ExecuteAsync(Client request, CancellationToken cancellationToken);
}

public sealed class AddClientUseCase(IContext context) : IAddClientUseCase
{
    private readonly IContext _context = context;

    public Task<Client> ExecuteAsync(Client request, CancellationToken cancellationToken)
    {
        var clientId = Guid.NewGuid();
        var client = new Client
        {
            Id = clientId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address
        };

        _context.Clients.Add(client);

        return Task.FromResult(client);
    }
}
