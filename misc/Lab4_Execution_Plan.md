# Lab 4 Execution Plan (tailored to current codebase)

## 1) Current state (what already exists)

This plan is based on your current project state in `Asp_projekt`.

### Already implemented
- Attribute routing is in place (`[Route("[controller]")]` + `[HttpGet]/[HttpPost]`).
- List + details pages exist for all main entities:
  - `Fakultet`, `Profesor`, `Student`, `Kolegij`, `Ocjena`, `Izvjestaj`.
- `Ocjena` has partial CRUD:
  - `Create` GET/POST exists.
  - `Delete` POST exists.
  - `Edit` does not exist.
- `Ocjena/Create` has some custom JS behavior (Napomena card updates on dropdown changes).
- Server-side DB setup, migrations, seeding already work.

### Missing for Lab 4 requirements
- Full CRUD coverage is missing for most entities.
- AJAX search is missing on all list pages.
- Custom autocomplete dropdown (AJAX-backed) is missing.
- Validation is not consistently implemented across create/edit forms.
- Advanced JS animations are not yet systematically implemented.
- Date-time custom control (partial view, non-native datepicker, hr+en support) is missing.

---

## 2) Scope decisions to make before implementation

To avoid rework, we will lock these decisions first:

1. Which entities must have full CRUD in UI:
   - Recommended: `Fakultet`, `Profesor`, `Student`, `Kolegij`, `Ocjena`.
   - `Izvjestaj` can be read-only only if justified by business rule (derived data), otherwise add CRUD too.
2. Date-time picker library choice:
   - Recommended stable option: `flatpickr` (+ locale packs), wrapped in a shared partial.
3. Autocomplete target field:
   - Recommended first implementation: `Student` form -> `Kolegij` selection via AJAX autocomplete dropdown.
   - Then reuse same control for at least one more relation (`Ocjena` form for `Profesor` or `Student`).

---

## 3) Implementation strategy (phases)

## Phase A - Baseline and safety net
Goal: create a safe starting point and prove current app is stable before changes.

Tasks:
- Run build and app smoke test.
- Snapshot current endpoint/view matrix.
- Create a short checklist file for tracking Lab 4 criteria.

Deliverable:
- Verified clean baseline and actionable TODO checklist.

---

## Phase B - Complete CRUD per entity (controllers + forms)
Goal: satisfy "potpuno funkcionalne stranice za pregled, pretragu, unos, uređivanje i brisanje".

### B1) Add missing Create/Edit/Delete actions
For each target controller:
- Add `Create` GET/POST.
- Add `Edit` GET/POST.
- Add `Delete` POST (or keep existing if already present).
- Keep attribute routing style consistent with project:
  - `[HttpGet("Create")]`, `[HttpPost("Create")]`
  - `[HttpGet("Edit/{id:int}")]`, `[HttpPost("Edit/{id:int}")]`
  - `[HttpPost("Delete/{id:int}")]`

### B2) Use form view models (not raw entities) for Create/Edit
- Introduce dedicated `Create` and `Edit` form models where needed.
- Re-map server-side explicitly into tracked entities.
- For edit POST, either:
  - map DTO -> fetched entity, or
  - use `TryUpdateModelAsync` with explicit included fields.

### B3) Views for Create/Edit
- Add strongly typed Create/Edit views per entity.
- Add anti-forgery token on all POST forms.
- Keep visual style aligned with existing UI (cards/hero/consistent button style).

### B4) Business-rule-safe delete behavior
- For entities with relational constraints, ensure delete path is deterministic:
  - either blocked with user message,
  - or controlled cascade behavior,
  - or soft-delete (optional, only if needed).

Deliverable:
- Functional CRUD routes and forms for selected entities, without breaking current pages.

---

## Phase C - Validation (client + server)
Goal: pass required validation criterion fully.

Tasks:
- Add DataAnnotations on form models (or entities where appropriate):
  - `[Required]`, `[StringLength]`, `[Range]`, etc.
- In POST actions enforce:
  - `if (!ModelState.IsValid) return View(model);`
- Ensure all forms render validation summaries/messages in styled way.
- Ensure unobtrusive validation scripts are loaded and active.
- Add blur-trigger behavior where needed for immediate UX feedback.

Deliverable:
- Consistent validation behavior on blur + on submit + server fallback.

---

## Phase D - AJAX search on every list page
Goal: every list page supports async search.

Target pages:
- `Views/Fakultet/Index.cshtml`
- `Views/Profesor/Index.cshtml`
- `Views/Student/Index.cshtml`
- `Views/Kolegij/Index.cshtml`
- `Views/Ocjena/Index.cshtml`
- `Views/Izvjestaj/Index.cshtml`

Tasks:
- Add per-controller search endpoint, e.g. `[HttpGet("Search")]` returning partial HTML.
- Extract list cards/table body into reusable partial for each entity.
- Add search input + debounce JS + fetch/jQuery GET to replace list container.
- Keep fallback server rendering for first page load.

Deliverable:
- AJAX search works on every index page without full page reload.

---

## Phase E - Custom AJAX autocomplete dropdown
Goal: implement reusable custom dropdown with async suggestions.

Tasks:
- Create reusable partial, e.g. `Views/Shared/_AutocompleteDropdown.cshtml`.
- Add generic JS module in `wwwroot/js/site.js` (or separate file loaded in layout).
- Add endpoint(s) returning JSON suggestions:
  - `id`, `label`, optional metadata.
- Use hidden input for selected ID, visible input for typed label.
- Keyboard/mouse support:
  - arrow keys, enter, escape, click outside.
- Integrate in Create + Edit forms (at least one critical relation, preferably two).

Deliverable:
- One reusable autocomplete control used in real forms with AJAX backend.

---

## Phase F - Date-time control via partial view (non-native)
Goal: satisfy date control requirement everywhere dates are edited.

Tasks:
- Add date-time partial (shared), e.g. `Views/Shared/_DateTimePicker.cshtml`.
- Use JS date-time picker plugin (recommended `flatpickr`) OR full custom picker.
- Do NOT use browser default date input UI.
- Use browser language/culture for format:
  - `hr` -> `d.m.Y H:i`
  - `en` -> `m/d/Y h:i K` or consistent en format.
- Replace all date inputs in Create/Edit forms:
  - `Ocjena.DatumOcjene`
  - `Student.DatumUpisa`
  - `Izvjestaj.DatumGeneriranja` if editable in CRUD scope.

Deliverable:
- Shared date-time control partial used across all relevant forms.

---

## Phase G - Advanced JS animations (functional, not decorative only)
Goal: satisfy advanced JS criterion with purpose-driven animations.

Tasks:
- Add meaningful animations for:
  - AJAX search result update transitions,
  - autocomplete open/close and highlight,
  - validation error reveal and resolution.
- Use accessible motion:
  - respect reduced-motion preference.

Deliverable:
- Noticeable, purposeful JS-driven motion that supports UX.

---

## Phase H - Final QA and submission readiness
Goal: ensure no hidden point loss.

Checklist:
- Build succeeds.
- All CRUD endpoints reachable and working.
- AJAX search works on each list page.
- Autocomplete works in create + edit path.
- Validation works client + server.
- Date-time picker works in hr and en browser settings.
- No broken navigation/routes/views.

Deliverable:
- Final verification matrix and ready-to-submit branch state.

---

## 4) Entity-by-entity gap map (quick)

Current inferred status:
- `Ocjena`: Index/Details/Create/Delete done; Edit missing; no AJAX search.
- `Profesor`: Index/Details only.
- `Student`: Index/Details only.
- `Kolegij`: Index/Details only.
- `Fakultet`: Index/Details only.
- `Izvjestaj`: Index/Details only (confirm whether business rule allows edit/create/delete).

This plan prioritizes finishing one entity end-to-end first, then replicating pattern safely.

---

## 5) Exact prompts you can send me (copy/paste)

Use these in order for best results.

1. `Start Lab4 Phase A: do a full readiness audit of current CRUD/search/validation/date-control coverage, produce a concise gap report, and create a task checklist file in root.`

2. `Start Lab4 Phase B for Ocjena first: implement full CRUD including Edit GET/POST with a proper edit view model, keep current style, and run build.`

3. `Now generalize the CRUD pattern from Ocjena to Profesor, Student, Kolegij, and Fakultet (controllers + view models + Create/Edit views + Delete forms), then run build and fix errors.`

4. `For Izvjestaj, evaluate whether full CRUD is business-valid; if yes implement it, if no implement a read-only rule with explicit UI and server safeguards and explain the rationale in code comments.`

5. `Implement Lab4 Phase C validation across all Create/Edit forms: DataAnnotations, server ModelState checks, styled validation messages, and blur-triggered client validation.`

6. `Implement Lab4 Phase D: add AJAX search to every Index page using search endpoints + partial result rendering + debounced JS updates.`

7. `Implement Lab4 Phase E: build a reusable AJAX autocomplete dropdown partial and wire it into at least Student and Ocjena forms for related-entity selection.`

8. `Implement Lab4 Phase F: add a shared non-native datetime picker partial (flatpickr acceptable), apply it to all date fields in Create/Edit forms, and ensure hr/en format behavior.`

9. `Implement Lab4 Phase G: add purposeful JS animations for AJAX result updates, autocomplete interactions, and validation states (respect reduced-motion).`

10. `Run full Lab4 Phase H QA: verify every rubric item, run build, test key routes, and produce a final pass/fail checklist with any remaining blockers.`

11. `Prepare final commit set for Lab4: group commits by phase, write clear messages, and show me the exact git commands before executing them.`

---

## 6) Terminal commands you can run yourself for quick verification

From repo root (`/Users/martinsvazic/Desktop/ASP`):

```bash
dotnet build Asp_projekt/Asp_projekt.csproj
dotnet run --project Asp_projekt/Asp_projekt.csproj
```

Optional endpoint smoke checks (while app running):
```bash
curl -I http://localhost:5000/Ocjena
curl -I http://localhost:5000/Profesor
curl -I http://localhost:5000/Student
curl -I http://localhost:5000/Kolegij
curl -I http://localhost:5000/Fakultet
curl -I http://localhost:5000/Izvjestaj
```

---

## 7) Risk notes specific to this repository

- Current routing is pure attribute routing; keep all new endpoints attribute-based.
- Existing custom UI styling should be preserved (avoid regressing into default Bootstrap forms).
- Date picker requirement explicitly forbids browser default datepicker; do not use plain `input type="date"` as final state.
- `Ocjena/Create` already has custom JS logic; integrate new components without breaking current behavior.
- Seeded relational data is richer now (multi-professor, multiple students per course), so CRUD forms must validate relational consistency.

---

## 8) Recommended execution order for minimal risk

1. Complete one entity fully (`Ocjena`) and lock the pattern.
2. Clone pattern to `Student` and `Profesor`.
3. Extend to `Kolegij` and `Fakultet`.
4. Decide `Izvjestaj` strategy.
5. Add shared infrastructure (AJAX search, autocomplete component, datetime partial).
6. Final polish + QA + submission.

This order minimizes repeated rework and aligns with your existing code structure.
