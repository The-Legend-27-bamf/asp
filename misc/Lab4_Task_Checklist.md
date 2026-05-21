# Lab4 Task Checklist (Execution Tracker)

Date started: 2026-05-21

## A) Phase A - Audit

- [x] Read Lab4 requirements in `Lab4.md`
- [x] Audit current controller endpoint coverage
- [x] Audit views/forms coverage
- [x] Audit validation coverage (client + server)
- [x] Audit AJAX search/autocomplete readiness
- [x] Audit date-time control readiness
- [x] Create gap report in root (`Lab4_PhaseA_Gap_Report.md`)

## B) Full CRUD by Entity

### B1. Ocjena
- [x] Add Edit GET endpoint
- [x] Add Edit POST endpoint
- [x] Add edit view model
- [x] Add `Views/Ocjena/Edit.cshtml`
- [x] Ensure server validation + anti-forgery in edit flow

### B2. Fakultet
- [x] Add Create GET/POST
- [x] Add Edit GET/POST
- [x] Add Delete POST
- [x] Add create/edit view models
- [x] Add `Views/Fakultet/Create.cshtml`
- [x] Add `Views/Fakultet/Edit.cshtml`
- [x] Add Create/Edit/Delete UI actions from index/details pages

### B3. Profesor
- [x] Add Create GET/POST
- [x] Add Edit GET/POST
- [x] Add Delete POST
- [x] Add create/edit view models
- [x] Add `Views/Profesor/Create.cshtml`
- [x] Add `Views/Profesor/Edit.cshtml`
- [x] Add Create/Edit/Delete UI actions from index/details pages

### B4. Student
- [x] Add Create GET/POST
- [x] Add Edit GET/POST
- [x] Add Delete POST
- [x] Add create/edit view models
- [x] Add `Views/Student/Create.cshtml`
- [x] Add `Views/Student/Edit.cshtml`
- [x] Add Create/Edit/Delete UI actions from index/details pages

### B5. Kolegij
- [x] Add Create GET/POST
- [x] Add Edit GET/POST
- [x] Add Delete POST
- [x] Add create/edit view models
- [x] Add `Views/Kolegij/Create.cshtml`
- [x] Add `Views/Kolegij/Edit.cshtml`
- [x] Add Create/Edit/Delete UI actions from index/details pages

### B6. Izvjestaj
- [x] Decide rule: full CRUD vs read-only derived entity
- [ ] If CRUD: implement Create/Edit/Delete + views + validation
- [x] If read-only: add explicit server + UI guard and documented rationale

## C) Validation (Client + Server)

- [x] Add DataAnnotations on all create/edit view models
- [x] Add `ModelState.IsValid` checks on all POST Create/Edit actions
- [x] Ensure anti-forgery on all POST forms
- [x] Ensure validation messages render in all forms
- [x] Ensure `_ValidationScriptsPartial` is included where needed
- [x] Verify validation triggers on blur and on submit

## D) AJAX Search on Every List Page

- [x] Add Search endpoint in `FakultetController`
- [x] Add Search endpoint in `ProfesorController`
- [x] Add Search endpoint in `StudentController`
- [x] Add Search endpoint in `KolegijController`
- [x] Add Search endpoint in `OcjenaController`
- [x] Add Search endpoint in `IzvjestajController`
- [x] Add search input to every index view
- [x] Extract list body/card rendering into partials for async refresh
- [x] Implement debounced AJAX client logic for each index page
- [x] Verify no full-page refresh during search

## E) Reusable AJAX Autocomplete Dropdown

- [x] Create reusable shared partial (e.g. `_AutocompleteDropdown.cshtml`)
- [x] Create generic JS behavior in `wwwroot/js/site.js` or dedicated JS file
- [x] Add JSON search endpoint(s) for dropdown suggestions
- [x] Integrate autocomplete in at least one Create + one Edit form
- [x] Integrate autocomplete in second entity relation for reuse proof
- [x] Verify keyboard navigation + selection + hidden ID binding

## F) Date-Time Control (Partial View, Non-Native, hr+en)

- [x] Select approach (recommended: flatpickr)
- [x] Add shared partial (e.g. `_DateTimePicker.cshtml`)
- [x] Add required JS/CSS assets
- [x] Replace native `type="date"` controls in all Create/Edit forms
- [x] Ensure date-time works in HR format when browser locale is hr
- [x] Ensure date-time works in EN format when browser locale is en
- [x] Verify parsing + model binding on server after submit

## G) Advanced JavaScript Usage

- [x] Add purposeful animation for AJAX search result updates
- [x] Add purposeful animation for autocomplete open/select states
- [x] Add purposeful validation feedback transitions
- [x] Respect reduced motion preferences

## H) Final QA

- [x] Build succeeds (`dotnet build`)
- [x] App runs (`dotnet run`)
- [x] CRUD works for all required entities
- [x] AJAX search works on every list page
- [x] Autocomplete dropdown works via AJAX
- [x] Validation works client + server
- [x] Date-time partial control works (non-native, hr+en)
- [x] No broken routes or view runtime errors

## Notes

- Keep routing style attribute-based only.
- Preserve existing visual language and CSS patterns.
- Reuse components (search partial pattern, autocomplete partial, datetime partial) to reduce duplication.
