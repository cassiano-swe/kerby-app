namespace Kerby.Api.Modules.Webhook.Features.AddEndpoint;

using Kerby.Api.Abstractions;
using Kerby.Api.Exceptions;
using Microsoft.EntityFrameworkCore;

public interface IAddEndpointUseCase
{
    Task<Domain.Endpoint> ExecuteAsync(Domain.Endpoint request, CancellationToken cancellationToken);
}

public sealed class AddEndpointUseCase : IAddEndpointUseCase
{
    private readonly IContext _context;

    public AddEndpointUseCase(IContext context)
    {
        _context = context;
    }

    public async Task<Domain.Endpoint> ExecuteAsync(Kerby.Api.Modules.Webhook.Domain.Endpoint request, CancellationToken cancellationToken)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken)
            ?? throw new NotFoundException(nameof(Client), request.ClientId);
        
        var endpointId = Guid.NewGuid();
        var endpoint = new Domain.Endpoint
        {
            Id = endpointId,
            ClientId = request.ClientId,
            Name = request.Name,
            Provider = request.Provider,
            DestinationUrl = request.DestinationUrl,
            PublicToken = Guid.NewGuid().ToString("N"),
            CreatedAt = DateTime.UtcNow,
        };

        _context.Endpoints.Add(endpoint);
        await _context.SaveChangesAsync(cancellationToken);

        return endpoint;
    }
}
