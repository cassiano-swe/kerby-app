namespace Kerby.Api.Modules.Client;

using Kerby.Api.Modules.Client.Features.AddClient;
using Kerby.Api.Modules.Client.Features.GetClient;

public static class ClientModule
{
    public static IEndpointRouteBuilder MapClientEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapAddClientEndpoint();
        endpoints.MapGetClientEndpoint();

        return endpoints;
    }
}
