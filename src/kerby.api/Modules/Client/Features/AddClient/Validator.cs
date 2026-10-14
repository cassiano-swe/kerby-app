namespace Kerby.Api.Modules.Client.Features.AddClient;

using FluentValidation;

public sealed class AddClientRequestValidator : AbstractValidator<AddClientRequest>
{
    public AddClientRequestValidator()
    {
        RuleFor(request => request.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(request => request.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(request => request.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(request => request.PhoneNumber)
            .NotEmpty()
            .Matches(@"^\+?[1-9]\d{1,14}$");
    }
}
