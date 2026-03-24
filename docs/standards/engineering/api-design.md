# API Design

## Endpoints

| Method | Path | Success | Errors |
|--------|------|---------|--------|
| GET | `/menu` | 200 — drinks + customisation options | — |
| POST | `/orders` | 201 — `{ id, orderNumber, totalPrice }` | 400 validation, 402 declined, 503 unavailable |
| GET | `/orders/active` | 200 — array of active orders | — |
| POST | `/orders/{id}/complete` | 200 | 404 not found |

## Status codes

| Code | Meaning |
|------|---------|
| 200 | Success |
| 201 | Created (new order) |
| 400 | Validation error (bad input) |
| 402 | Payment declined |
| 404 | Not found |
| 503 | Payment gateway unavailable |

## Error shape

All error responses use a consistent JSON object:

```json
{ "error": "<message>" }
```

Not a bare string — always a JSON object with an `error` property.

## Conventions

- JSON only — no XML, no content negotiation.
- DTOs are the HTTP contract — domain types are internal to the OrderApi project.
- Handlers are thin — delegate to domain services for business logic.
- No authentication — this is a reference example.
