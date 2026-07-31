namespace Kerby.Api.Modules.Client;

using Kerby.Api.Modules.Client.Features.AddClient;
using Kerby.Api.Modules.Client.Features.DeleteClient;
using Kerby.Api.Modules.Client.Features.GetClient;
using Kerby.Api.Modules.Client.Features.UpdateClient;

public static class ClientModule
{
    public static IEndpointRouteBuilder MapClientEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapAddClientEndpoint();
        endpoints.MapGetClientEndpoint();
        endpoints.MapDeleteClientEndpoint();
        endpoints.MapUpdateClientEndpoint();

        return endpoints;
    }
}
