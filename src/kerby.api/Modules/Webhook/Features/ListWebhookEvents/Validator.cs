namespace Kerby.Api.Modules.Webhook.Features.ListWebhookEvents;

using FluentValidation;
using Kerby.Api.Modules.Webhook.Domain;

public sealed class ListWebhookEventsRequestValidator
    : AbstractValidator<ListWebhookEventsRequest>
{
    public ListWebhookEventsRequestValidator()
    {
        RuleFor(request => request.ClientId)
            .Must(clientId => !clientId.HasValue || clientId.Value != Guid.Empty)
            .WithMessage("ClientId must be a valid GUID.");

        RuleFor(request => request.EndpointId)
            .Must(endpointId => !endpointId.HasValue || endpointId.Value != Guid.Empty)
            .WithMessage("EndpointId must be a valid GUID.");

        RuleFor(request => request.Status)
            .Must(
                status =>
                    string.IsNullOrWhiteSpace(status)
                    || Enum.TryParse<DeliveryStatus>(status, ignoreCase: true, out _)
            )
            .WithMessage("Status must be Pending, Succeeded, or Failed.");

        RuleFor(request => request.StartDate)
            .Must(
                (request, startDate) =>
                    !startDate.HasValue
                    || !request.EndDate.HasValue
                    || startDate.Value <= request.EndDate.Value
            )
            .WithMessage("StartDate must be earlier than or equal to EndDate.");

        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(1)
            .When(request => request.Page.HasValue);

        RuleFor(request => request.PageSize)
            .InclusiveBetween(1, 100)
            .When(request => request.PageSize.HasValue);
    }
}
