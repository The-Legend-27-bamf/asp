using Asp_projekt.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Asp_projekt.Controllers;

public class StudentController : Controller
{
    private readonly StudentMockRepository _studentRepository;

    public StudentController(StudentMockRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public IActionResult Index()
    {
        return View(_studentRepository.GetAll());
    }

    public IActionResult Details(int id)
    {
        var student = _studentRepository.GetById(id);
        if (student is null)
        {
            return NotFound();
        }

        return View(student);
    }
}