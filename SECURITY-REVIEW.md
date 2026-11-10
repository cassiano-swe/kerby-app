# Code review findings

Review scope: application security, correctness, and performance. Tests were not
run, as requested.

## Security

### HIGH — Unauthenticated client and webhook management

- **Files:** `src/kerby.api/Program.cs:29-31,49-53`;
  `src/kerby.api/Modules/Client/ClientModule.cs:12-17`;
  `src/kerby.api/Modules/Webhook/WebhookModule.cs:11-23`
- **Confidence:** 9/10
- **Finding:** The application does not configure authentication or authorization,
  and these routes have no authorization requirements. If the service is
  reachable from an untrusted network, unauthenticated users can read client
  contact details, change or delete clients, register webhooks, and access
  webhook-event metadata.

## Correctness

### LOW — Large page numbers can overflow the query offset

- **Files:** `src/kerby.api/Modules/Webhook/Features/ListWebhookEvents/ListWebhookEventsUseCase.cs:91`;
  `src/kerby.api/Modules/Webhook/Features/ListWebhookEvents/Validator.cs:36-42`
- **Confidence:** 9/10
- **Finding:** `Page` has no upper bound, while the query offset uses `int`
  multiplication. A sufficiently large page value can overflow to a negative
  offset and cause the database query to fail.

## Performance

### LOW — Webhook-event listing does not have an index for its sort order

- **Files:** `src/kerby.api/Modules/Webhook/Features/ListWebhookEvents/ListWebhookEventsUseCase.cs:87-92`;
  `src/kerby.api/Infrastructure/Data/Migrations/20261108164059_InitialPostgres.cs:77-85`
- **Confidence:** 8/10
- **Finding:** Listing counts matching events and sorts by `ReceivedAt` and `Id`,
  but the initial migration does not define an index for that order. As the event
  history grows, the listing query may require increasingly expensive scans and
  sorting. The exact count is also performed for every listing request.
