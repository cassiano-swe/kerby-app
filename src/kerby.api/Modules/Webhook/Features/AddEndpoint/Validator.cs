namespace Kerby.Api.Modules.Webhook.Features.AddEndpoint;

using FluentValidation;

public sealed class AddEndpointRequestValidator : AbstractValidator<AddEndpointRequest>
{
    public AddEndpointRequestValidator()
    {
        RuleFor(request => request.ClientId)
            .NotEqual(Guid.Empty)
            .WithMessage("ClientId must be a valid GUID.");

        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(request => request.Provider)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(request => request.DestinationUrl)
            .NotEmpty()
            .Must(value => Uri.TryCreate(value, UriKind.Absolute, out _))
            .WithMessage("Destination URL must be a valid absolute URL.");
    }
}
