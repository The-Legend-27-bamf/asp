# Lab4 Phase A - Readiness Audit (Current State)

Date: 2026-05-21
Project: Asp_projekt (ASP.NET Core MVC + EF Core)

## Summary

Overall readiness for Lab 4 is partial.

- Routing and base read pages are stable.
- Only one entity (`Ocjena`) has partial write flow (Create/Delete).
- Required Lab4 features (AJAX list search, reusable AJAX autocomplete dropdown, reusable date-time partial control) are not implemented yet.
- Validation is currently implemented only for `Ocjena` create flow.

## Rubric Coverage Matrix

| Lab4 criterion | Current status | Notes |
| --- | --- | --- |
| Full CRUD support for all relevant entities | FAIL | `Fakultet`, `Profesor`, `Student`, `Kolegij`, `Izvjestaj` have only Index/Details; `Ocjena` lacks Edit |
| AJAX search on every list page | FAIL | No `Search` endpoints in controllers; no custom AJAX list refresh in app views |
| Dropdown with AJAX autocomplete | FAIL | No autocomplete partial and no JSON search endpoint for dropdown suggestions |
| Validation (client + server) | PARTIAL | Present for `Ocjena/Create`; absent for rest of forms because forms do not yet exist |
| Advanced JavaScript usage | PARTIAL | Custom JS exists in `Ocjena/Create` (Napomena update), but no app-wide advanced JS tied to Lab4 tasks |
| Date control via partial (datetime, non-native, hr+en) | FAIL | No shared date-time partial; currently using native `type="date"` in `Ocjena/Create` |

## Controller Endpoint Audit

### Current endpoints by entity
- `FakultetController`: GET Index, GET Details
- `ProfesorController`: GET Index, GET Details
- `StudentController`: GET Index, GET Details
- `KolegijController`: GET Index, GET Details
- `IzvjestajController`: GET Index, GET Details
- `OcjenaController`: GET Index, GET Create, POST Create, GET Details, POST Delete

### Missing endpoints for full CRUD
- Missing Create/Edit/Delete for all entities except `Ocjena`.
- Missing Edit for `Ocjena`.
- Missing AJAX search endpoints (`GET Search`) for all list controllers.

## View Audit

### Existing views
- For each main entity, only `Index` + `Details` exist.
- `Ocjena` additionally has `Create`.

### Missing views for CRUD completion
- Missing `Create` and `Edit` views for: `Fakultet`, `Profesor`, `Student`, `Kolegij`, `Izvjestaj`.
- Missing `Edit` view for `Ocjena`.

## Validation Audit

- Validation UI (`asp-validation-for`, `asp-validation-summary`) and `_ValidationScriptsPartial` usage found only in `Views/Ocjena/Create.cshtml`.
- Server-side `ModelState.IsValid` check + anti-forgery are present in `OcjenaController` create flow.
- No equivalent form validation patterns exist for other entities (no forms yet).

## AJAX/Search/JS Audit

- No controller search endpoints (`HttpGet("Search")`) found.
- No app-level custom AJAX search implementation found in index pages.
- Custom JS exists only in `Ocjena/Create.cshtml` for selection summary updates.
- `wwwroot/js/site.js` is effectively empty placeholder.

## Date Control Audit

- `Ocjena/Create` uses native browser date input (`type="date"`).
- No shared date-time partial exists in `Views/Shared`.
- No non-native date-time picker plugin integration exists.
- No explicit hr/en date-time picker behavior currently implemented.

## Key Risks Before Implementation

1. If CRUD is added quickly without shared patterns, code duplication will be high and harder to maintain.
2. If AJAX search is done differently on each index page, quality and reliability will vary.
3. Date control requirement explicitly forbids default browser picker; this must be solved with a shared custom/JS control early.
4. `Izvjestaj` may be conceptually derived data; CRUD decision must be explicit to avoid rubric ambiguity.

## Recommended Next Step

Proceed with Phase B starting from one reference entity (`Ocjena`), complete full CRUD + reusable patterns, then replicate to remaining entities.
