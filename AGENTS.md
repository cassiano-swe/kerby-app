# AI Agent Instructions — Kerby API

These instructions apply to the entire repository. Read them before planning or modifying code. Their purpose is to guide agents such as Codex across different machines and sessions, keeping changes compatible with the architecture and the actual state of the project.

## Project purpose

Kerby is a REST API built with ASP.NET Core and .NET 10. It currently provides CRUD operations for clients and allows registering webhook endpoint configurations associated with clients. The implementation **does not** receive or forward webhook events.

See `README.md` for the developer-facing overview. When information conflicts, treat the code and project file as the sources of truth.

## Repository map

```text
kerby-app.slnx
README.md
AGENTS.md
src/
└── kerby.api/
    ├── kerby.api.csproj
    ├── Program.cs
    ├── appsettings.json
    ├── appsettings.Development.json
    ├── kerby.api.http
    ├── Properties/launchSettings.json
    ├── Abstractions/
    │   └── IContext.cs
    ├── Exceptions/
    │   └── GlobalExceptionHandler.cs
    ├── Infrastructure/
    │   └── Data/Context.cs
    └── Modules/
        ├── Client/
        │   ├── ClientModule.cs
        │   ├── Domain/ (Client.cs, ClientValidator.cs)
        │   └── Features/
        │       ├── AddClient/
        │       ├── GetClient/
        │       ├── UpdateClient/
        │       └── DeleteClient/
        └── Webhook/
            ├── WebhookModule.cs
            ├── Domain/ (Endpoint.cs)
            └── Features/AddEndpoint/
```

Each feature folder generally contains `Endpoint.cs`, the use case/interface, and `Validator.cs`. Some parts are incomplete; always verify the actual contents before assuming that validation or behavior is implemented.

## Existing architecture and conventions

- **Modular Minimal APIs:** `Program.cs` configures the application and invokes each module's mapping methods. `ClientModule` groups routes under `/api/clients`; `WebhookModule` groups routes under `/api/webhooks`.
- **Feature-based organization:** keep each operation under `Modules/<Module>/Features/<Action>/`. Do not introduce a generic controller layer when modular Minimal APIs meet the requirement.
- **Thin endpoint + use case:** handlers bind HTTP data, call an injected use-case interface, and convert the result to `IResult`. Put application logic in the use case, not the handler.
- **Dependency injection:** use-case interfaces and implementations are explicitly registered as `Scoped` in `Program.cs`. Follow the existing pattern and register new use cases when adding them.
- **Persistence:** use cases depend on `IContext`; `Context` is the EF Core `DbContext` implementation and exposes `DbSet<Client>` and `DbSet<Endpoint>`. `IContext` exposes EF Core-specific types; do not describe or treat it as a technology-independent repository.
- **Entities:** current models are mutable entities with `Guid` identifiers. Preserve property names and existing style unless the task requires a contract change.
- **Asynchronous operations:** use async APIs for I/O and pass the endpoint's `CancellationToken` through the use case and persistence calls.
- **Error handling:** global exceptions and their conversion to Problem Details are handled in `Exceptions/GlobalExceptionHandler.cs`. Before changing thrown exception types, check the mappings so HTTP status codes remain intentional.

## Current routes and behavior

- `POST /api/clients/`: creates a client with `firstName`, `lastName`, `email`, and `phoneNumber`.
- `GET /api/clients/{id}`: retrieves a client; returns `404` when not found.
- `PUT /api/clients/{id}`: updates the client's fields.
- `DELETE /api/clients/{id}`: deletes a client.
- `POST /api/webhooks/`: registers a client, name, provider, and destination URL; generates a public token and creation timestamp.

Consider these points before changing or documenting the API contracts:

- The client-creation response currently sets `Location` to `/clients/{id}`, which does not match the mapped `/api/clients/{id}` route.
- Webhook creation sets `Location` to `/endpoints/{id}`; no corresponding retrieval endpoint exists.
- The `/in/{token}` URL returned when registering a webhook is informational: no inbound route is implemented. Events are not forwarded either.
- The update and delete use cases currently throw `InvalidOperationException` when a client does not exist; the global handler does not map it to `404`, so it falls through to the generic `500` case.
- Do not assume automatic validation: FluentValidation is a dependency and `ClientValidator` contains rules, but feature validators are empty and no integration that registers or invokes them was found.
- `src/kerby.api/kerby.api.http` may be out of date with respect to routes and the launch profile's port. Check it before using it as a reference.

Do not fix these limitations incidentally when unrelated to the request. If a task requires fixing one, update the behavior intentionally, add tests when possible, and keep the README and HTTP examples in sync.

## Persistence and configuration

- The project targets `net10.0`, with nullable reference types and implicit usings enabled.
- EF Core is configured with the InMemory provider and database name `kerbydb` in `Program.cs`. Data does not survive an application restart.
- No relational provider, connection string, or migrations have been identified in the current project. Do not claim persistent database support unless it is implemented.
- OpenAPI and Scalar are configured only in the `Development` environment. The `http` profile in `Properties/launchSettings.json` uses `http://localhost:8081`.
- Never put credentials, real tokens, or secrets in source code, committed configuration, or documentation.

## Dependencies

The versions declared in `src/kerby.api/kerby.api.csproj` are the source of truth:

- FluentValidation 12.1.1
- Microsoft.AspNetCore.OpenApi 10.0.10
- Microsoft.EntityFrameworkCore 10.0.10
- Microsoft.EntityFrameworkCore.InMemory 10.0.10
- Microsoft.OpenApi 2.11.0
- Scalar.AspNetCore 2.16.11

Before introducing a package, check whether the requirement can be met with existing dependencies or the framework. When adding or removing a dependency, update the `.csproj` and keep the solution restorable; do not invent versions or edit `obj/` or `bin/` manually.

## Recommended change workflow

1. Read this file and the files directly related to the request. Verify the current structure; do not rely solely on this description or the README.
2. Identify the affected feature, endpoint, use case, module, `Program.cs` registration, entity/persistence, and error handling.
3. Make the smallest change consistent with local conventions. Avoid broad refactoring or unrequested contract changes.
4. For a new route, keep the handler thin, create or update the feature, map it in the appropriate module, and register the use case at startup. Update related documentation and examples.
5. Validate with a build and relevant tests. Do not claim to have run checks that were not run. If no applicable tests exist or the SDK is unavailable, state the limitation clearly.
6. Review the diff for generated files, unrelated changes, and inconsistent documentation.

## Useful commands

Run these from the repository root; the .NET 10 SDK is required:

```bash
dotnet restore kerby-app.slnx
dotnet build kerby-app.slnx
dotnet test kerby-app.slnx
dotnet run --project src/kerby.api/kerby.api.csproj --launch-profile http
```

No test project has currently been found in the repository; `dotnet test` may not run any tests. When adding tests, use a project structure and framework compatible with the solution and include the project in the solution. With the `http` profile, the API runs at `http://localhost:8081`; Scalar is at `/api-reference` and OpenAPI at `/openapi/v1.json` in the development environment.

## Change scope and safety

- Modify only what is necessary to fulfill the user's request.
- Do not modify generated files (`bin/`, `obj/`, `src/kerby.api/.tmp/`) or rely on machine-specific paths; these instructions must work in any checkout.
- Do not run destructive commands, create commits, or alter Git history unless explicitly requested.
- Preserve API compatibility where possible. Intentional changes to routes, payloads, or HTTP status codes must be reflected in the documentation.
- At completion, briefly summarize what changed and which validations were or were not performed.
