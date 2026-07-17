namespace Kerby.Api.Modules.Client;

using Kerby.Api.Modules.Client.Features.AddClient;

public static class ClientModule
{
    public static IEndpointRouteBuilder MapClientEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapAddClientEndpoint();

        return endpoints;
    }
}