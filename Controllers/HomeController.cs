using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Controllers;

public class HomeController : Controller
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _mapper;

    public HomeController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _db = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var courses = await _db.Courses.GetAllAsync();
        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(courses);
        return View(courseViewModels);
    }

    public IActionResult Create()
    {
        ViewData["Categories"] = _db.Category.GetAllAsync().Result;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Course course)
    {
        if (!ModelState.IsValid)
            return View(course);
        await _db.Courses.AddAsync(course);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
