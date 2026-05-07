# Sitemap (URLs → Controller/Action/View)

This sitemap lists the MVC endpoints currently exposed by the app.

Routing is configured in `Asp_projekt/Program.cs` with:

- Attribute routing: `app.MapControllers()`
- Conventional routing: `{controller=Home}/{action=Index}/{id?}`

Because both are enabled, most actions are reachable through their attribute route *and* (typically) through the conventional route.

## Home

### GET /
- Controller: `HomeController`
- Action: `Index`
- View: `Views/Home/Index.cshtml`
- Route source: conventional default route (`controller=Home`, `action=Index`)

### GET /Home
- Controller: `HomeController`
- Action: `Index`
- View: `Views/Home/Index.cshtml`
- Route source: attribute route (`[Route("[controller]")]` + `[HttpGet("")]`)

### GET /Home/Index
- Controller: `HomeController`
- Action: `Index`
- View: `Views/Home/Index.cshtml`
- Route source: conventional

### GET /Home/Privacy
- Controller: `HomeController`
- Action: `Privacy`
- View: `Views/Home/Privacy.cshtml`
- Route source: attribute (`[HttpGet("Privacy")]`) + conventional

### GET /Home/Error
- Controller: `HomeController`
- Action: `Error`
- View: `Views/Shared/Error.cshtml`
- Route source: attribute (`[HttpGet("Error")]`) + conventional

## Fakultet

### GET /Fakultet
- Controller: `FakultetController`
- Action: `Index`
- View: `Views/Fakultet/Index.cshtml`
- Route source: attribute (`[HttpGet("")]`) + conventional

### GET /Fakultet/{id:int}
- Controller: `FakultetController`
- Action: `Details(int id)`
- View: `Views/Fakultet/Details.cshtml`
- Route source: attribute (`[HttpGet("{id:int}")]`)

### Conventional equivalents (if using `{controller}/{action}/{id?}`)
- GET /Fakultet/Index
- GET /Fakultet/Details/{id?}

## Profesor

### GET /Profesor
- Controller: `ProfesorController`
- Action: `Index`
- View: `Views/Profesor/Index.cshtml`

### GET /Profesor/{id:int}
- Controller: `ProfesorController`
- Action: `Details(int id)`
- View: `Views/Profesor/Details.cshtml`

### Conventional equivalents
- GET /Profesor/Index
- GET /Profesor/Details/{id?}

## Student

### GET /Student
- Controller: `StudentController`
- Action: `Index`
- View: `Views/Student/Index.cshtml`

### GET /Student/{id:int}
- Controller: `StudentController`
- Action: `Details(int id)`
- View: `Views/Student/Details.cshtml`

### Conventional equivalents
- GET /Student/Index
- GET /Student/Details/{id?}

## Kolegij

### GET /Kolegij
- Controller: `KolegijController`
- Action: `Index`
- View: `Views/Kolegij/Index.cshtml`

### GET /Kolegij/{id:int}
- Controller: `KolegijController`
- Action: `Details(int id)`
- View: `Views/Kolegij/Details.cshtml`

### Conventional equivalents
- GET /Kolegij/Index
- GET /Kolegij/Details/{id?}

## Ocjena

### GET /Ocjena
- Controller: `OcjenaController`
- Action: `Index`
- View: `Views/Ocjena/Index.cshtml`

### GET /Ocjena/Create
- Controller: `OcjenaController`
- Action: `Create()`
- View: `Views/Ocjena/Create.cshtml`

### POST /Ocjena/Create
- Controller: `OcjenaController`
- Action: `Create(OcjenaCreateViewModel model)`
- View: `Views/Ocjena/Create.cshtml` (on validation errors)

### GET /Ocjena/{id:int}
- Controller: `OcjenaController`
- Action: `Details(int id)`
- View: `Views/Ocjena/Details.cshtml`

### Conventional equivalents
- GET /Ocjena/Index
- GET /Ocjena/Create
- GET /Ocjena/Details/{id?}

## Izvjestaj

### GET /Izvjestaj
- Controller: `IzvjestajController`
- Action: `Index`
- View: `Views/Izvjestaj/Index.cshtml`

### GET /Izvjestaj/{id:int}
- Controller: `IzvjestajController`
- Action: `Details(int id)`
- View: `Views/Izvjestaj/Details.cshtml`

### Conventional equivalents
- GET /Izvjestaj/Index
- GET /Izvjestaj/Details/{id?}

## Not included

- Static assets (e.g. `/css/site.css`, `/js/site.js`, `/lib/...`) are served from `wwwroot/` and `app.MapStaticAssets()` and are not listed here since they do not map to controller/actions/views.
