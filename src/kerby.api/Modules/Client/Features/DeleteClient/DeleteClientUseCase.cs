namespace Kerby.Api.Modules.Client.Features.DeleteClient;

using Kerby.Api.Abstractions;
using Kerby.Api.Modules.Client.Domain;
using Microsoft.EntityFrameworkCore;

public interface IDeleteClientUseCase
{
    Task ExecuteAsync(Guid id, CancellationToken cancellationToken);
}

public sealed class DeleteClientUseCase(IContext context) : IDeleteClientUseCase
{
    private readonly IContext _context = context;

    public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken)
    {
        var client = await _context
            .Clients.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (client != null)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
