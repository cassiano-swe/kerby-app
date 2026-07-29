namespace Kerby.Api.Modules.Client.Features.GetClient;

using Kerby.Api.Abstractions;
using Kerby.Api.Modules.Client.Domain;
using Microsoft.EntityFrameworkCore;

public interface IGetClientUseCase
{
    Task<Client?> ExecuteAsync(Guid id, CancellationToken cancellationToken);
}

public sealed class GetClientUseCase(IContext context) : IGetClientUseCase
{
    private readonly IContext _context = context;

    public async Task<Client?> ExecuteAsync(Guid id, CancellationToken cancellationToken)
    {
        var client = await _context.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return client;
    }
}
