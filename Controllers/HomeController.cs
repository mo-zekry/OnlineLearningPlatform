using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Controllers;

public class HomeController : BaseController
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _mapper;

    public HomeController(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        UserManager<ApplicationUser> userManager
    )
        : base(userManager)
    {
        _db = unitOfWork;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        var courses = _db.Courses.Get();

        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(courses);

        var categories = _db.Categories.Get();
        var categoryViewModels = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
        ViewData["Categories"] = categoryViewModels;

        return View(courseViewModels);
    }

    [HttpPost]
    public IActionResult Search(string query)
    {
        var results = _db.Courses.Get(filter: x => x.Name.Contains(query));

        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(results);

        var categories = _db.Categories.Get();
        var categoryViewModels = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
        ViewData["Categories"] = categoryViewModels;

        return PartialView("_CourseListPartial", courseViewModels);
    }

    [HttpPost]
    public IActionResult GetCourses()
    {
        var results = _db.Courses.Get();

        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(results);

        var categories = _db.Categories.Get();
        var categoryViewModels = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
        ViewData["Categories"] = categoryViewModels;

        return PartialView("_CourseListPartial", courseViewModels);
    }

    [HttpPost]
    public IActionResult Filter(int id)
    {
        var results = _db.Courses.Get(filter: x => x.CategoryId == id);

        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(results);

        var categories = _db.Categories.Get();
        var categoryViewModels = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
        ViewData["Categories"] = categoryViewModels;

        return PartialView("_CourseListPartial", courseViewModels);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
