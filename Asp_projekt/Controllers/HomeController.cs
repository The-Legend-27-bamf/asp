using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Asp_projekt.Models;
using Asp_projekt.Repositories;

namespace Asp_projekt.Controllers;

public class HomeController : Controller
{
    private readonly StudentMockRepository _studentRepository;
    private readonly KolegijMockRepository _kolegijRepository;
    private readonly ProfesorMockRepository _profesorRepository;
    private readonly OcjenaMockRepository _ocjenaRepository;
    private readonly FakultetMockRepository _fakultetRepository;
    private readonly IzvjestajMockRepository _izvjestajRepository;

    public HomeController(
        StudentMockRepository studentRepository,
        KolegijMockRepository kolegijRepository,
        ProfesorMockRepository profesorRepository,
        OcjenaMockRepository ocjenaRepository,
        FakultetMockRepository fakultetRepository,
        IzvjestajMockRepository izvjestajRepository)
    {
        _studentRepository = studentRepository;
        _kolegijRepository = kolegijRepository;
        _profesorRepository = profesorRepository;
        _ocjenaRepository = ocjenaRepository;
        _fakultetRepository = fakultetRepository;
        _izvjestajRepository = izvjestajRepository;
    }

    public IActionResult Index()
    {
        var studenti = _studentRepository.GetAll();
        var kolegiji = _kolegijRepository.GetAll();
        var profesori = _profesorRepository.GetAll();
        var ocjene = _ocjenaRepository.GetAll();
        var fakulteti = _fakultetRepository.GetAll();
        var izvjestaji = _izvjestajRepository.GetAll();

        var randomFakultetIndex = fakulteti.Count > 0 ? Random.Shared.Next(fakulteti.Count) : -1;

        var model = new HomeDashboardViewModel
        {
            Student = PickRandom(studenti),
            Kolegij = PickRandom(kolegiji),
            Profesor = PickRandom(profesori),
            Ocjena = PickRandom(ocjene),
            Fakultet = randomFakultetIndex >= 0 ? fakulteti[randomFakultetIndex] : null,
            FakultetId = randomFakultetIndex >= 0 ? randomFakultetIndex + 1 : null,
            Izvjestaj = PickRandom(izvjestaji)
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private static T? PickRandom<T>(List<T> items) where T : class
    {
        if (items.Count == 0)
        {
            return null;
        }

        return items[Random.Shared.Next(items.Count)];
    }
}
