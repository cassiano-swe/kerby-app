namespace Kerby.Api.Modules.Client.Features.UpdateClient;

using Kerby.Api.Abstractions;
using Kerby.Api.Modules.Client.Domain;

public interface IUpdateClientUseCase
{
    Task<Client> ExecuteAsync(Client request, CancellationToken cancellationToken);
}

public sealed class UpdateClientUseCase(IContext context) : IUpdateClientUseCase
{
    private readonly IContext _context = context;

    public async Task<Client> ExecuteAsync(Client request, CancellationToken cancellationToken)
    {
        var client = await _context.Clients.FindAsync(request.Id, cancellationToken) ?? throw new InvalidOperationException("Client not found");

        _context.Entry(client).CurrentValues.SetValues(request);

        await _context.SaveChangesAsync(cancellationToken);

        return client;
    }
}
