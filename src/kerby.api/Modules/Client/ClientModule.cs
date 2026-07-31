namespace Kerby.Api.Modules.Client;

using Kerby.Api.Modules.Client.Features.AddClient;
using Kerby.Api.Modules.Client.Features.DeleteClient;
using Kerby.Api.Modules.Client.Features.GetClient;
using Kerby.Api.Modules.Client.Features.UpdateClient;

public static class ClientModule
{
    public static IEndpointRouteBuilder MapClientEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/clients").WithTags("Clients");

        group.MapAddClientEndpoint();
        group.MapGetClientEndpoint();
        group.MapDeleteClientEndpoint();
        group.MapUpdateClientEndpoint();

        return group;
    }
}
